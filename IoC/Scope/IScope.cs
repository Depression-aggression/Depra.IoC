using System;

namespace Depra.IoC.Scope
{
    public interface IScope : IDisposable
    {
        object Resolve(Type service);
    }
}