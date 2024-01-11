// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depra.IoC.Activation;
using Depra.IoC.Description;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;
using Depra.IoC.Scope;

namespace Depra.IoC
{
	public sealed class Container : IContainer
	{
		private readonly Scope _rootScope;
		private readonly IActivationBuilder _activationBuilder;
		private readonly ConcurrentDictionary<Type, ServiceDescriptor> _descriptors;
		private readonly ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>> _buildActivators;

		public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder activationBuilder)
		{
			Guard.AgainstNull(descriptors, nameof(descriptors));
			Guard.AgainstNull(activationBuilder, nameof(activationBuilder));

			_rootScope = new Scope(this);
			_activationBuilder = activationBuilder;
			_descriptors = new ConcurrentDictionary<Type, ServiceDescriptor>();
			_buildActivators = new ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>>();

			FillDescriptors(descriptors);
		}

		public void Dispose()
		{
			_rootScope.Dispose();
			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			await _rootScope.DisposeAsync().ConfigureAwait(false);
			GC.SuppressFinalize(this);
		}

		public IScope CreateScope() => new Scope(this);

		private ServiceDescriptor FindDescriptor(Type service)
		{
			if (_descriptors.TryGetValue(service, out var descriptor))
			{
				return descriptor;
			}

			if (service.IsAssignableFrom(typeof(IEnumerable)) && service.IsGenericType &&
			    service.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				var items = FindDescriptor(service.GetGenericArguments().Single());
				return items != null
					? _descriptors.GetOrAdd(service, BuildUsingMultipleDescriptor(service, items))
					: null;
			}

			if (service.IsConstructedGenericType == false)
			{
				return null;
			}

			var genericTypeDefinition = service.GetGenericTypeDefinition();
			var genericDescriptor = FindDescriptor(genericTypeDefinition);
			if (!(genericDescriptor is TypeBasedServiceDescriptor typeBased))
			{
				return null;
			}

			var genericArguments = service.GetGenericArguments();
			var genericType = typeBased.ImplementationType.MakeGenericType(genericArguments);
			var argumentsDescriptor = new TypeBasedServiceDescriptor(genericType, service, typeBased.Lifetime);

			return _descriptors.GetOrAdd(genericType, argumentsDescriptor);
		}

		private object CreateInstance(IScope scope, ServiceDescriptor descriptor) =>
			_buildActivators.GetOrAdd(descriptor, _ => BuildActivation(descriptor, _activationBuilder))(scope);

		private static Func<IScope, object> BuildActivation(ServiceDescriptor serviceDescriptor,
			IActivationBuilder activationBuilder) => serviceDescriptor switch
		{
			InstanceBasedServiceDescriptor instanceBased => _ => instanceBased.Instance,
			FactoryBasedServiceDescriptor factoryBased => factoryBased.Factory,
			_ => activationBuilder.BuildActivation((TypeBasedServiceDescriptor) serviceDescriptor)
		};

		private void FillDescriptors(IEnumerable<ServiceDescriptor> descriptors)
		{
			var descriptorsAsDictionary = (IDictionary<Type, ServiceDescriptor>) _descriptors;
			foreach (var descriptorsGroup in descriptors.GroupBy(x => x.Type))
			{
				var items = descriptorsGroup.ToArray();
				if (items.Length == 1)
				{
					descriptorsAsDictionary.Add(descriptorsGroup.Key, items[0]);
				}
				else
				{
					const LifetimeType lifetime = (LifetimeType) int.MaxValue;
					var multiple = new MultipleServicesDescriptor(descriptorsGroup.Key, lifetime, items);
					descriptorsAsDictionary.Add(descriptorsGroup.Key, multiple);
					var serviceType = typeof(IEnumerable<>).MakeGenericType(descriptorsGroup.Key);
					descriptorsAsDictionary.Add(serviceType, BuildUsingMultipleDescriptor(serviceType, multiple));
				}
			}
		}

		private static ServiceDescriptor BuildUsingMultipleDescriptor(Type serviceType, ServiceDescriptor descriptor) =>
			new FactoryBasedServiceDescriptor(serviceType, LifetimeType.TRANSIENT, scope =>
			{
				var items = (descriptor as MultipleServicesDescriptor)?.Descriptors ?? new[] { descriptor };
				var scopeImpl = (Scope) scope;
				var array = Array.CreateInstance(descriptor.Type, items.Length);
				for (var index = 0; index < array.Length; index++)
				{
					array.SetValue(scopeImpl.ResolveInternal(items[index]), index);
				}

				return array;
			});

		private sealed class Scope : IScope
		{
			private readonly Container _container;
			private readonly ConcurrentStack<object> _disposables;
			private readonly ConcurrentDictionary<ServiceDescriptor, object> _scopedInstances;

			public Scope(Container container)
			{
				_container = container;
				_disposables = new ConcurrentStack<object>();
				_scopedInstances = new ConcurrentDictionary<ServiceDescriptor, object>();
			}

			public void Dispose()
			{
				foreach (var @object in _disposables)
				{
					if (@object is IAsyncDisposable asyncDisposable)
					{
						Task.Run(async () => await asyncDisposable.DisposeAsync().ConfigureAwait(false))
							.ConfigureAwait(false)
							.GetAwaiter()
							.GetResult();
					}
					else
					{
						((IDisposable) @object).Dispose();
					}
				}
			}

			public async ValueTask DisposeAsync()
			{
				foreach (var @object in _disposables)
				{
					if (@object is IAsyncDisposable asyncDisposable)
					{
						await asyncDisposable.DisposeAsync();
					}
					else
					{
						((IDisposable) @object).Dispose();
					}
				}
			}

			public object Resolve(Type service)
			{
				var descriptor = _container.FindDescriptor(service);
				Guard.AgainstNull(descriptor, () => new UnableFindRegistration(service));

				return ResolveInternal(descriptor);
			}

			internal object ResolveInternal(ServiceDescriptor descriptor) =>
				descriptor.Lifetime == LifetimeType.TRANSIENT
					? CreateInstanceInternal(descriptor)
					: descriptor.Lifetime == LifetimeType.SCOPED || _container._rootScope == this
						? _scopedInstances.GetOrAdd(descriptor, _ => CreateInstanceInternal(descriptor))
						: _container._rootScope.ResolveInternal(descriptor);

			private object CreateInstanceInternal(ServiceDescriptor descriptor)
			{
				var result = _container.CreateInstance(this, descriptor);
				if (result is IDisposable)
				{
					_disposables.Push(result);
				}

				if (result is IAsyncDisposable)
				{
					_disposables.Push(result);
				}

				return result;
			}
		}
	}
}