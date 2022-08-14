using System;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Structs;

namespace Depra.IoC.Descriptors.Impl
{
    public class InstanceBasedServiceDescriptor : ServiceDescriptor
    {
        public object Instance { get; }

        public InstanceBasedServiceDescriptor(Type serviceType, object instance) : base(serviceType, Lifetime.Singleton)
        {
            Instance = instance;
        }
    }
}