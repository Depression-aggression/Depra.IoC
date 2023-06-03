using System;
using Depra.IoC.Enums;

namespace Depra.IoC.Description
{
    public sealed class InstanceBasedServiceDescriptor : ServiceDescriptor
    {
        public InstanceBasedServiceDescriptor(Type serviceType, object instance) :
            base(serviceType, LifetimeType.Singleton) =>
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));

        public object Instance { get; }
    }
}