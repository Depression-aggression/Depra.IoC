using System;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Scope;
using Depra.IoC.Structs;

namespace Depra.IoC.Descriptors.Impl
{
    public class FactoryBasedServiceDescriptor : ServiceDescriptor
    {
        public Func<IScope, object> Factory { get; }

        public FactoryBasedServiceDescriptor(Type serviceType, Lifetime lifetime, Func<IScope, object> factory) 
            : base(serviceType, lifetime)
        {
            Factory = factory;
        }
    }
}