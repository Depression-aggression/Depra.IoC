using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Depra.IoC.Application.Activation.Interfaces;
using Depra.IoC.Application.Descriptors;
using Depra.IoC.Application.Disposing;
using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Containers
{
    public abstract class ContainerBase : DisposableObject, IContainer
    {
        private readonly IActivationBuilder _builder;
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>> _buildActivators;

        protected internal abstract ScopeBase RootScope { get; }

        public abstract IScope CreateScope();

        protected ContainerBase(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder)
        {
            _builder = builder;
            _descriptors = new ConcurrentDictionary<Type, ServiceDescriptor>();
            _buildActivators = new ConcurrentDictionary<ServiceDescriptor, Func<IScope, object>>();

            FillDescriptors(descriptors);
        }

        protected override void ReleaseManagedResources()
        {
            RootScope.Dispose();
        }

        internal object CreateInstance(IScope scope, ServiceDescriptor descriptor) =>
            _buildActivators.GetOrAdd(descriptor, _ => BuildActivation(descriptor, _builder))(scope);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service">Founded service. Can be null!</param>
        /// <returns></returns>
        protected internal ServiceDescriptor FindDescriptor(Type service)
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
            if (genericDescriptor is TypeBasedServiceDescriptor typeBased == false)
            {
                return null;
            }

            var genericArguments = service.GetGenericArguments();
            var genericType = typeBased.ImplementationType.MakeGenericType(genericArguments);
            var argumentsDescriptor = new TypeBasedServiceDescriptor(genericType, service, typeBased.Lifetime);
            return _descriptors.GetOrAdd(genericType, argumentsDescriptor);
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
                    var multiple =
                        new MultipleServicesDescriptor(descriptorsGroup.Key, (LifetimeType)int.MaxValue, items);
                    descriptorsAsDictionary.Add(descriptorsGroup.Key, multiple);
                    var serviceType = typeof(IEnumerable<>).MakeGenericType(descriptorsGroup.Key);
                    descriptorsAsDictionary.Add(serviceType, BuildUsingMultipleDescriptor(serviceType, multiple));
                }
            }
        }

        private static ServiceDescriptor BuildUsingMultipleDescriptor(Type serviceType, ServiceDescriptor descriptor)
        {
            return new FactoryBasedServiceDescriptor(serviceType, LifetimeType.Transient, scope =>
            {
                var items = (descriptor as MultipleServicesDescriptor)?.Descriptors ?? new[] { descriptor };
                var scopeImpl = (ScopeBase)scope;
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