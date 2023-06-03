using System;
using Depra.IoC.Enums;

namespace Depra.IoC.Description
{
    public abstract class ServiceDescriptor
    {
        protected ServiceDescriptor(Type serviceType, LifetimeType lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }

        public LifetimeType Lifetime { get; }
    }
}