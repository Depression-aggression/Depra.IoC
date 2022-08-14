using Depra.IoC.Containers.Impl;
using Depra.IoC.Containers.Interfaces;
using Depra.IoC.Scope.Activation.Impl;
using Depra.IoC.Scope.Containers.Builders.Abstract;

namespace Depra.IoC.Containers.Builders.Impl
{
    public class ReflectionBasedContainerBuilder : ContainerBuilder
    {
        public override IContainer Build() => new Container(Descriptors, new ReflectionBasedActivationBuilder());
    }
}