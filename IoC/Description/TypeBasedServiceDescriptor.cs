using System;
using Depra.IoC.Enums;

namespace Depra.IoC.Description
{
    public sealed class TypeBasedServiceDescriptor : ServiceDescriptor
    {
        public TypeBasedServiceDescriptor(Type implementationType, Type serviceType, LifetimeType lifetime) :
            base(serviceType, lifetime) =>
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));

        public Type ImplementationType { get; }
    }
}