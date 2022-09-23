using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Domain.Extensions
{
    public static class ScopeExtensions
    {
        public static TService Resolve<TService>(this IScope scope) => (TService)scope.Resolve(typeof(TService));
    }
}