using System.Reflection;
using Depra.IoC.Injection.Factory;
using Depra.IoC.Injection.Delegates;

namespace Depra.IoC.Injection.Factory
{
    public sealed class ReflectionBasedInjectorFactory : IInjectionFactory
    {
        public ConstructorInjector Create(ConstructorInfo constructor) => constructor.Invoke;

        public PropertyInjector Create(PropertyInfo property) =>
            (target, value) => property.SetValue(target, value, null);

        public MethodInjector Create(MethodInfo method) => 
            (target, args) => method.Invoke(target, args);
    }
}