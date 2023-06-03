using System;
using Depra.IoC.Enums;
using Depra.IoC.Scope;

namespace Depra.IoC.Description
{
    public sealed class FactoryBasedServiceDescriptor : ServiceDescriptor
    {
        public FactoryBasedServiceDescriptor(Type serviceType, LifetimeType lifetime, Func<IScope, object> factory) :
            base(serviceType, lifetime) =>
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));

        public Func<IScope, object> Factory { get; }
    }
}