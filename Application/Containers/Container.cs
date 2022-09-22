using System.Collections.Generic;
using Depra.IoC.Application.Activation.Interfaces;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Containers
{
    public class Container : ContainerBase
    {
        private class Scope : ScopeBase
        {
            public Scope(ContainerBase container) : base(container) { }
        }

        private readonly Scope _rootScope;

        protected internal override ScopeBase RootScope => _rootScope;

        public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder) :
            base(descriptors, builder)
        {
            _rootScope = CreateScopeInternal();
        }

        public override IScope CreateScope() => CreateScopeInternal();

        private Scope CreateScopeInternal() => new Scope(this);
    }
}