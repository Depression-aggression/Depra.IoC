using System;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.New.Async
{
    public interface IAsyncScope : IScope, IAsyncDisposable { }
}