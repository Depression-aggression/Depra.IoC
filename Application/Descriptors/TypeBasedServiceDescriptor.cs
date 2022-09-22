using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;

namespace Depra.IoC.Application.Descriptors
{
    public class TypeBasedServiceDescriptor : ServiceDescriptor
    {
        public Type ImplementationType { get; }

        public TypeBasedServiceDescriptor(Type implementationType, Type serviceType, LifetimeType lifetime) : base(
            serviceType, lifetime)
        {
            ImplementationType = implementationType;
        }
    }
}