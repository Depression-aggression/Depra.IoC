using System;
using Depra.IoC.Enums;

namespace Depra.IoC.Description
{
    public sealed class MultipleServicesDescriptor : ServiceDescriptor
    {
        public MultipleServicesDescriptor(
            Type serviceType,
            LifetimeType lifetime,
            ServiceDescriptor[] descriptors) : base(serviceType, lifetime) =>
            Descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));

        public ServiceDescriptor[] Descriptors { get; }
    }
}