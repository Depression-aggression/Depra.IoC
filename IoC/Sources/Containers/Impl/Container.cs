using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depra.IoC.Activation.Interfaces;
using Depra.IoC.Containers.Interfaces;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Descriptors.Impl;
using Depra.IoC.Scope;
using Depra.IoC.Structs;

namespace Depra.IoC.Containers.Impl
{
    public class Container : IContainer
    {
        private class Scope : IScope
        {
            private readonly Container _container;
            private readonly ConcurrentDictionary<ServiceDescriptor, object> _scopedInstances;
            private readonly ConcurrentStack<object> _disposables;

            public object Resolve(Type service)
            {
                var descriptor = _container.FindDescriptor(service);
                if (descriptor == null)
                {
                    throw new InvalidOperationException($"Unable to find registration for {service}");
                }

                return ResolveInternal(descriptor);
            }

            internal object ResolveInternal(ServiceDescriptor descriptor)
            {
                if (descriptor.Lifetime == Lifetime.Transient)
                {
                    return CreateInstanceInternal(descriptor);
                }

                var isRootScope = _container._rootScope == this;
                if (descriptor.Lifetime == Lifetime.Scoped || isRootScope)
                {
                    return _scopedInstances.GetOrAdd(descriptor, _ => CreateInstanceInternal(descriptor));
                }

                return _container._rootScope.ResolveInternal(descriptor);
            }

            public Scope(Container container)
            {
                _container = container;
                _scopedInstances = new ConcurrentDictionary<ServiceDescriptor, object>();
                _disposables = new ConcurrentStack<object>();
            }

            private object CreateInstanceInternal(ServiceDescriptor descriptor)
            {
                var result = _container.CreateInstance(this, descriptor);
                if (result is IDisposable or IAsyncDisposable)
                {
                    _disposables.Push(result);
                }

                return result;
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
                        ((IDisposable)@object).Dispose();
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
                        ((IDisposable)@object).Dispose();
                    }
                }
            }
        }

        private readonly Scope _rootScope;
        private readonly IActivationBuilder _builder;
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>> _buildActivators;

        public IScope CreateScope() => new Scope(this);

        public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder)
        {
            _builder = builder;
            _rootScope = new Scope(this);
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

        private ServiceDescriptor? FindDescriptor(Type service)
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
            if (genericDescriptor is not TypeBasedServiceDescriptor typeBased)
            {
                return null;
            }

            var genericArguments = service.GetGenericArguments();
            var genericType = typeBased.ImplementationType.MakeGenericType(genericArguments);
            var argumentsDescriptor = new TypeBasedServiceDescriptor(genericType, service, typeBased.Lifetime);
            return _descriptors.GetOrAdd(genericType, argumentsDescriptor);
        }

        private object CreateInstance(IScope scope, ServiceDescriptor descriptor)
        {
            return _buildActivators.GetOrAdd(descriptor, _ => BuildActivation(descriptor, _builder))(scope);
        }

        private static Func<IScope, object> BuildActivation(ServiceDescriptor descriptor,
            IActivationBuilder activationBuilder)
        {
            switch (descriptor)
            {
                case InstanceBasedServiceDescriptor instanceBased:
                    return _ => instanceBased.Instance;
                case FactoryBasedServiceDescriptor factoryBased:
                    return factoryBased.Factory;
            }

            var typeBased = (TypeBasedServiceDescriptor)descriptor;
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
                    var multiple = new MultipleServicesDescriptor(descriptorsGroup.Key, (Lifetime)int.MaxValue, items);
                    descriptorsAsDictionary.Add(descriptorsGroup.Key, multiple);
                    var serviceType = typeof(IEnumerable<>).MakeGenericType(descriptorsGroup.Key);
                    descriptorsAsDictionary.Add(serviceType, BuildUsingMultipleDescriptor(serviceType, multiple));
                }
            }
        }

        private static ServiceDescriptor BuildUsingMultipleDescriptor(Type serviceType, ServiceDescriptor descriptor)
        {
            return new FactoryBasedServiceDescriptor(serviceType, Lifetime.Transient, scope =>
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
        }
    }
}