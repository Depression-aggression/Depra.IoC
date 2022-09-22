using System;
using System.Collections.Concurrent;
using Depra.IoC.Application.Disposing;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Containers
{
    public abstract class ScopeBase : DisposableObject, IScope
    {
        private readonly ContainerBase _container;
        private readonly ConcurrentDictionary<ServiceDescriptor, object> _scopedInstances;
        protected ConcurrentStack<object> Disposables { get; }

        protected ScopeBase(ContainerBase container)
        {
            _container = container;
            Disposables = new ConcurrentStack<object>();
            _scopedInstances = new ConcurrentDictionary<ServiceDescriptor, object>();
        }

        public object Resolve(Type service)
        {
            var descriptor = _container.FindDescriptor(service);
            if (descriptor == null)
            {
                throw new InvalidOperationException($"Unable to find registration for {service}");
            }

            return ResolveInternal(descriptor);
        }

        protected override void ReleaseManagedResources()
        {
            foreach (var @object in Disposables)
            {
                ((IDisposable)@object).Dispose();
            }
        }

        internal object ResolveInternal(ServiceDescriptor descriptor)
        {
            if (descriptor.Lifetime == LifetimeType.Transient)
            {
                return CreateInstanceInternal(descriptor);
            }

            var isRootScope = _container.RootScope == this;
            if (descriptor.Lifetime == LifetimeType.Scoped || isRootScope)
            {
                return _scopedInstances.GetOrAdd(descriptor, _ => CreateInstanceInternal(descriptor));
            }

            return _container.RootScope.ResolveInternal(descriptor);
        }

        private object CreateInstanceInternal(ServiceDescriptor descriptor)
        {
            var result = _container.CreateInstance(this, descriptor);
            if (result is IDisposable)
            {
                Disposables.Push(result);
            }

            return result;
        }
    }
}