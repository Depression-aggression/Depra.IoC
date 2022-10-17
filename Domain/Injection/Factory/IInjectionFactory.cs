using System.Reflection;
using Depra.IoC.Domain.Injection.Delegates;

namespace Depra.IoC.Domain.Injection.Factory
{
    public interface IInjectionFactory
    {
        ConstructorInjector Create(ConstructorInfo constructor);

        PropertyInjector Create(PropertyInfo property);

        MethodInjector Create(MethodInfo method);
    }
}