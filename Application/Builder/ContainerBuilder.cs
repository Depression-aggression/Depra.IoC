using System.Collections.Generic;
using Depra.IoC.Application.Activation;
using Depra.IoC.Application.Containers;
using Depra.IoC.Domain.Builder;
using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Description;

namespace Depra.IoC.Application.Builder
{
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly List<ServiceDescriptor> _descriptors;
        private readonly IActivationBuilder _activationBuilder;

        public IContainer Build() => new Container(_descriptors, _activationBuilder);

        public void Register(ServiceDescriptor descriptor) => _descriptors.Add(descriptor);

        public ContainerBuilder(IActivationBuilder activationBuilder)
        {
            _activationBuilder = activationBuilder;
            _descriptors = new List<ServiceDescriptor>();
        }

        public override string ToString() => GetType().Name;
    }
}