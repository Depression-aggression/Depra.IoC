using System.Collections.Generic;
using Depra.IoC.Application.Containers.Builders.Interfaces;
using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Description;

namespace Depra.IoC.Application.Containers.Builders.Abstract
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

        public override string ToString() => GetType().Name;
    }
}