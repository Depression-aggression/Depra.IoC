namespace Depra.IoC.Scope
{
    public static class ScopeExtensions
    {
        public static TService Resolve<TService>(this IScope scope) => (TService)scope.Resolve(typeof(TService));
    }
}