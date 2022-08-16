using Depra.IoC.Activation.Impl;
using Depra.IoC.Containers.Builders.Abstract;
using Depra.IoC.Containers.Impl;
using Depra.IoC.Containers.Interfaces;

namespace Depra.IoC.Containers.Builders.Impl
{
    public class ReflectionBasedContainerBuilder : ContainerBuilder
    {
        public override IContainer Build() => new Container(Descriptors, new ReflectionBasedActivationBuilder());
    }
}