using Depra.IoC.Activation.Impl;
using Depra.IoC.Application.Containers;
using Depra.IoC.Application.Containers.Builders.Abstract;
using Depra.IoC.Domain.Container;

namespace Depra.IoC.Containers.Builders.Impl
{
    public class LambdaBasedContainerBuilder : ContainerBuilder
    {
        public override IContainer Build() => new Container(Descriptors, new LambdaBasedActivationBuilder());
    }
}