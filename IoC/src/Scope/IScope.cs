using System;

namespace Depra.IoC.Scope
{
    public interface IScope : IDisposable, IAsyncDisposable
    {
        object Resolve(Type service);
    }
}