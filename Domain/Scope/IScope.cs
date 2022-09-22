using System;

namespace Depra.IoC.Domain.Scope
{
    public interface IScope : IDisposable
    {
        object Resolve(Type service);
    }
}