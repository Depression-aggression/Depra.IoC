using System.Collections.Generic;
using Depra.IoC.Containers.Builders.Interfaces;
using Depra.IoC.Containers.Interfaces;
using Depra.IoC.Descriptors.Abstract;

namespace Depra.IoC.Containers.Builders.Abstract
{
    public abstract class ContainerBuilder : IContainerBuilder
    {
        protected List<ServiceDescriptor> Descriptors { get; }

        public void Register(ServiceDescriptor descriptor)
        {
            Descriptors.Add(descriptor);
        }

        public abstract IContainer Build();

        protected ContainerBuilder()
        {
            Descriptors = new List<ServiceDescriptor>();
        }
    }
}