using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Depra.IoC.Application.Activation.Interfaces;
using Depra.IoC.Application.Containers;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.New.Async
{
    public class AsyncContainer : ContainerBase, IAsyncContainer
    {
        private readonly AsyncScope _rootScope;

        protected override ScopeBase RootScope => _rootScope;

        public override IScope CreateScope() => CreateScopeInternal();

        public AsyncContainer(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder) :
            base(descriptors, builder)
        {
            _rootScope = CreateScopeInternal();
        }

        public async ValueTask DisposeAsync()
        {
            await _rootScope.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        private AsyncScope CreateScopeInternal() => new(this);
    }
}