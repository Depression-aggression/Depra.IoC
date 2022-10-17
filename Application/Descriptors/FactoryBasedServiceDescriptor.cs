using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Descriptors
{
    public class FactoryBasedServiceDescriptor : ServiceDescriptor
    {
        public Func<IScope, object> Factory { get; }

        public FactoryBasedServiceDescriptor(Type serviceType, LifetimeType lifetime, Func<IScope, object> factory) :
            base(serviceType, lifetime)
        {
            Factory = factory;
        }
    }
}