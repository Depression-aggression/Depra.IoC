using System;
using System.Collections.Generic;
using Depra.IoC.Activation;
using Depra.IoC.Container;
using Depra.IoC.Description;

namespace Depra.IoC.Builder
{
    public sealed class ContainerBuilder : IContainerBuilder
    {
        private readonly List<ServiceDescriptor> _descriptors;
        private readonly IActivationBuilder _activationBuilder;

        public ContainerBuilder(IActivationBuilder activationBuilder)
        {
            _activationBuilder = activationBuilder ?? throw new ArgumentNullException(nameof(activationBuilder));
            _descriptors = new List<ServiceDescriptor>();
        }

        public IContainer Build() => 
            new Container.Container(_descriptors, _activationBuilder);

        public void Register(ServiceDescriptor descriptor) =>
            _descriptors.Add(descriptor);

        public override string ToString() => GetType().Name;
    }
}