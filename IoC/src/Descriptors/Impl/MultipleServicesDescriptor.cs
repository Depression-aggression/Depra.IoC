using System;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Structs;

namespace Depra.IoC.Descriptors.Impl
{
    public class MultipleServicesDescriptor : ServiceDescriptor
    {
        public ServiceDescriptor[] Descriptors { get; }

        public MultipleServicesDescriptor(Type serviceType, Lifetime lifetime, ServiceDescriptor[] descriptors) : base(
            serviceType, lifetime)
        {
            Descriptors = descriptors;
        }
    }
}