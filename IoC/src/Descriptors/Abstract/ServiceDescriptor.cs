using System;
using Depra.IoC.Structs;

namespace Depra.IoC.Descriptors.Abstract
{
    public abstract class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public Lifetime Lifetime { get; }

        protected ServiceDescriptor(Type serviceType, Lifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }
    }
}