using System;
using System.Threading.Tasks;
using Depra.IoC.Application.Containers;

namespace Depra.IoC.Application.New.Async
{
    public class AsyncScope : ScopeBase, IAsyncScope
    {
        protected override void ReleaseManagedResources()
        {
            foreach (var @object in Disposables)
            {
                if (@object is IAsyncDisposable asyncDisposable)
                {
                    Task.Run(async () => await asyncDisposable.DisposeAsync().ConfigureAwait(false))
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }
                else
                {
                    ((IDisposable)@object).Dispose();
                }
            }
        }
        
        public async ValueTask DisposeAsync()
        {
            foreach (var @object in Disposables)
            {
                if (@object is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    ((IDisposable)@object).Dispose();
                }
            }
        }

        public AsyncScope(ContainerBase container) : base(container) { }
    }
}