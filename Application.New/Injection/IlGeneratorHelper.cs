using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Depra.IoC.Application.New.Injection
{
    internal static class IlGeneratorHelper
    {
        private static readonly MethodInfo UnboxPointer = typeof(Pointer).GetMethod("Unbox");
        
        public static void EmitLoadMethodArguments(ILGenerator ilGenerator, MethodBase targetMethod)
        {
            var parameters = targetMethod.GetParameters();
            var ldargOpcode = targetMethod is ConstructorInfo ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1;

            for (var i = 0; i < parameters.Length; i++)
            {
                ilGenerator.Emit(ldargOpcode);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
            }
        }

        public static void EmitMethodCall(ILGenerator ilGenerator, MethodInfo method)
        {
            var opCode = method.IsFinal ? OpCodes.Call : OpCodes.Callvirt;
            ilGenerator.Emit(opCode, method);
        }

        public static void EmitUnboxOrCast(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, type);
            }
            else if (type.IsPointer)
            {
                ilGenerator.Emit(OpCodes.Call, UnboxPointer);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, type);
            }
        }
    }
}