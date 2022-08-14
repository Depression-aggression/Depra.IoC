using System;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Structs;

namespace Depra.IoC.Descriptors.Impl
{
    public class TypeBasedServiceDescriptor : ServiceDescriptor
    {
        public Type ImplementationType { get; }

        public TypeBasedServiceDescriptor(Type implementationType, Type serviceType, Lifetime lifetime) : base(
            serviceType, lifetime)
        {
            ImplementationType = implementationType;
        }
    }
}