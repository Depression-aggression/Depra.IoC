using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;

namespace Depra.IoC.Application.Descriptors
{
    public class InstanceBasedServiceDescriptor : ServiceDescriptor
    {
        public object Instance { get; }

        public InstanceBasedServiceDescriptor(Type serviceType, object instance) : 
            base(serviceType, LifetimeType.Singleton)
        {
            Instance = instance;
        }
    }
}