using System;
using System.Reflection;
using System.Reflection.Emit;
using Depra.IoC.Domain.Injection;
using Depra.IoC.Injection.Delegates;

namespace Depra.IoC.Application.New.Injection
{
    public class DynamicMethodInjectorFactory : IInjectionFactory
    {
        private const bool INJECT_NON_PUBLIC = false;

        public ConstructorInjector Create(ConstructorInfo constructor)
        {
            var returnType = typeof(object);
            var parameterTypes = new[] { typeof(object[]) };
            var dynamicMethod = new DynamicMethod(GetAnonymousMethodName(), returnType, parameterTypes,
                constructor.Module, true);

            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Newobj, constructor);

            if (constructor.ReflectedType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, constructor.ReflectedType);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var injector = (ConstructorInjector)dynamicMethod.CreateDelegate(typeof(ConstructorInjector));

            return injector;
        }

        public PropertyInjector Create(PropertyInfo property)
        {
            var returnType = typeof(void);
            var parameterTypes = new[] { typeof(object), typeof(object) };
            var dynamicMethod = new DynamicMethod(GetAnonymousMethodName(), returnType, parameterTypes, property.Module,
                true);

            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            IlGeneratorHelper.EmitUnboxOrCast(ilGenerator, property.DeclaringType);

            ilGenerator.Emit(OpCodes.Ldarg_1);
            IlGeneratorHelper.EmitUnboxOrCast(ilGenerator, property.PropertyType);

            IlGeneratorHelper.EmitMethodCall(ilGenerator, property.GetSetMethod(INJECT_NON_PUBLIC));
            ilGenerator.Emit(OpCodes.Ret);

            var injector = (PropertyInjector)dynamicMethod.CreateDelegate(typeof(PropertyInjector));

            return injector;
        }

        public MethodInjector Create(MethodInfo method)
        {
            var returnType = typeof(void);
            var parameterTypes = new[] { typeof(object), typeof(object[]) };
            var dynamicMethod =
                new DynamicMethod(GetAnonymousMethodName(), returnType, parameterTypes, method.Module, true);

            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            IlGeneratorHelper.EmitUnboxOrCast(ilGenerator, method.DeclaringType);
            IlGeneratorHelper.EmitLoadMethodArguments(ilGenerator, method);
            IlGeneratorHelper.EmitMethodCall(ilGenerator, method);

            if (method.ReturnType != returnType)
            {
                ilGenerator.Emit(OpCodes.Pop);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var injector = (MethodInjector)dynamicMethod.CreateDelegate(typeof(MethodInjector));

            return injector;
        }


        private static string GetAnonymousMethodName() => "DynamicInjector" + Guid.NewGuid().ToString("N");
    }
}