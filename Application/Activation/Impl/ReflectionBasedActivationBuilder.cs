using System;
using System.Collections.Generic;
using System.Reflection;
using Depra.IoC.Application.Activation.Abstract;
using Depra.IoC.Domain.Scope;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation.Impl
{
    public class ReflectionBasedActivationBuilder : BaseActivationBuilder
    {
        protected override Func<IScope, object> BuildActivationInternal(ConstructorInfo constructor, ParameterInfo[] args)
        {
            return scope =>
            {
                var argsForConstructor = GetConstructorArguments(scope, args);
                var instance = constructor.Invoke(argsForConstructor);
                return instance;
            };
        }

        private static object[] GetConstructorArguments(IScope scope, IReadOnlyList<ParameterInfo> parameters)
        {
            if (parameters.Count == 0)
            {
                return Array.Empty<object>();
            }
            
            var arguments = new object[parameters.Count];
            for (var i = 0; i < parameters.Count; i++)
            {
                arguments[i] = scope.Resolve(parameters[i].ParameterType);
            }

            return arguments;
        }
    }
}