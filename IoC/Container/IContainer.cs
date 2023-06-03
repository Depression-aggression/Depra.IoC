using System;
using Depra.IoC.Scope;

namespace Depra.IoC.Container
{
    public interface IContainer : IDisposable
    {
        IScope CreateScope();
    }
}