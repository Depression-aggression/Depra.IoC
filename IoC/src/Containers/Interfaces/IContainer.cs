using System;
using Depra.IoC.Scope;

namespace Depra.IoC.Containers.Interfaces
{
    public interface IContainer : IDisposable, IAsyncDisposable
    {
        IScope CreateScope();
    }
}