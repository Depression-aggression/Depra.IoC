using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Enums;

namespace Depra.IoC.Application.Descriptors
{
    public class MultipleServicesDescriptor : ServiceDescriptor
    {
        public ServiceDescriptor[] Descriptors { get; }

        public MultipleServicesDescriptor(Type serviceType, LifetimeType lifetime, ServiceDescriptor[] descriptors) : base(
            serviceType, lifetime)
        {
            Descriptors = descriptors;
        }
    }
}