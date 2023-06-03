using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Depra.IoC.Activation;
using Depra.IoC.Description;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;
using Depra.IoC.Scope;
#if NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Depra.IoC.Container
{
    public sealed class Container : IContainer
    {
        private readonly Scope _rootScope;
        private readonly IActivationBuilder _activationBuilder;
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>> _buildActivators;

        public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder activationBuilder)
        {
            _rootScope = new Scope(this);
            _descriptors = new ConcurrentDictionary<Type, ServiceDescriptor>();
            _buildActivators = new ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>>();
            _activationBuilder = activationBuilder ?? throw new ArgumentNullException(nameof(activationBuilder));

            FillDescriptors(descriptors ?? throw new ArgumentNullException(nameof(descriptors)));
        }

        public IScope CreateScope() => new Scope(this);

        public void Dispose()
        {
            _rootScope.Dispose();
            GC.SuppressFinalize(this);
        }

#if NETSTANDARD2_1_OR_GREATER
        public async ValueTask DisposeAsync()
        {
            await _rootScope.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
#endif

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

        private static Func<IScope, object> BuildActivation(
            ServiceDescriptor serviceDescriptor,
            IActivationBuilder activationBuilder)
        {
            switch (serviceDescriptor)
            {
                case InstanceBasedServiceDescriptor instanceBased:
                    return _ => instanceBased.Instance;
                case FactoryBasedServiceDescriptor factoryBased:
                    return factoryBased.Factory;
            }

            var typeBased = (TypeBasedServiceDescriptor)serviceDescriptor;
            var activation = activationBuilder.BuildActivation(typeBased);

            return activation;
        }

        private void FillDescriptors(IEnumerable<ServiceDescriptor> descriptors)
        {
            var descriptorsAsDictionary = (IDictionary<Type, ServiceDescriptor>)_descriptors;
            foreach (var descriptorsGroup in descriptors.GroupBy(x => x.ServiceType))
            {
                var items = descriptorsGroup.ToArray();
                if (items.Length == 1)
                {
                    descriptorsAsDictionary.Add(descriptorsGroup.Key, items[0]);
                }
                else
                {
                    const LifetimeType lifetime = (LifetimeType)int.MaxValue;
                    var multiple = new MultipleServicesDescriptor(descriptorsGroup.Key, lifetime, items);
                    descriptorsAsDictionary.Add(descriptorsGroup.Key, multiple);
                    var serviceType = typeof(IEnumerable<>).MakeGenericType(descriptorsGroup.Key);
                    descriptorsAsDictionary.Add(serviceType, BuildUsingMultipleDescriptor(serviceType, multiple));
                }
            }
        }

        private static ServiceDescriptor BuildUsingMultipleDescriptor(Type serviceType, ServiceDescriptor descriptor) =>
            new FactoryBasedServiceDescriptor(serviceType, LifetimeType.Transient, scope =>
            {
                var items = (descriptor as MultipleServicesDescriptor)?.Descriptors ?? new[] { descriptor };
                var scopeImpl = (Scope)scope;
                var array = Array.CreateInstance(descriptor.ServiceType, items.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    array.SetValue(scopeImpl.ResolveInternal(items[i]), i);
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

            public object Resolve(Type service)
            {
                var descriptor = _container.FindDescriptor(service);
                if (descriptor == null)
                {
                    throw new UnableFindRegistrationException(service);
                }

                return ResolveInternal(descriptor);
            }

            internal object ResolveInternal(ServiceDescriptor descriptor)
            {
                if (descriptor.Lifetime == LifetimeType.Transient)
                {
                    return CreateInstanceInternal(descriptor);
                }

                var isRootScope = _container._rootScope == this;
                if (descriptor.Lifetime == LifetimeType.Scoped || isRootScope)
                {
                    return _scopedInstances.GetOrAdd(descriptor, _ => CreateInstanceInternal(descriptor));
                }

                return _container._rootScope.ResolveInternal(descriptor);
            }

            private object CreateInstanceInternal(ServiceDescriptor descriptor)
            {
                var result = _container.CreateInstance(this, descriptor);
                if (result is IDisposable)
                {
                    _disposables.Push(result);
                }

#if NETSTANDARD2_1_OR_GREATER
                if (result is IAsyncDisposable)
                {
                    _disposables.Push(result);
                }
#endif

                return result;
            }

            public void Dispose()
            {
                foreach (var @object in _disposables)
                {
#if NETSTANDARD2_1_OR_GREATER
                    if (@object is IAsyncDisposable asyncDisposable)
                    {
                        Task.Run(async () => await asyncDisposable.DisposeAsync().ConfigureAwait(false))
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();
                    }
                    else
#endif
                    {
                        ((IDisposable)@object).Dispose();
                    }
                }
            }

#if NETSTANDARD2_1_OR_GREATER
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
                        ((IDisposable)@object).Dispose();
                    }
                }
            }
#endif
        }
    }
}