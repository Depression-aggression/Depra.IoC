using System.Reflection;
using Depra.IoC.Domain.Injection.Delegates;
using Depra.IoC.Domain.Injection.Factory;

namespace Depra.IoC.Application.Injection
{
    public class ReflectionBasedInjectorFactory : IInjectionFactory
    {
        public ConstructorInjector Create(ConstructorInfo constructor) => constructor.Invoke;

        public PropertyInjector Create(PropertyInfo property) =>
            (target, value) => property.SetValue(target, value, null);

        public MethodInjector Create(MethodInfo method) => 
            (target, args) => method.Invoke(target, args);
    }
}