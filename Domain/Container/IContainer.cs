using System;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Domain.Container
{
    public interface IContainer : IDisposable
    {
        IScope CreateScope();
    }
}