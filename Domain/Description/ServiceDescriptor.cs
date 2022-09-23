using System;
using Depra.IoC.Domain.Enums;

namespace Depra.IoC.Domain.Description
{
    public abstract class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public LifetimeType Lifetime { get; }

        protected ServiceDescriptor(Type serviceType, LifetimeType lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }
    }
}