using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Depra.IoC.Activation.Abstract;

namespace Depra.IoC.Scope.Activation.Impl
{
    public class LambdaBasedActivationBuilder : BaseActivationBuilder
    {
        private static readonly MethodInfo ResolveMethod = typeof(IScope).GetMethod("Resolve");

        protected override Func<IScope, object> BuildActivationInternal(ConstructorInfo constructor,
            ParameterInfo[] args)
        {
            const string scopeParameterName = "scope";

            var scopeParameter = Expression.Parameter(typeof(IScope), scopeParameterName);
            var expressionArgs = args.Select(x => SelectExpressionArgs(x, scopeParameter));
            var @new = Expression.New(constructor, expressionArgs);
            var lambda = Expression.Lambda<Func<IScope, object>>(@new, scopeParameter);

            return lambda.Compile();
        }

        private static UnaryExpression SelectExpressionArgs(ParameterInfo parameterInfo, Expression parameterExpression)
        {
            var constantExpression = Expression.Constant(parameterInfo.ParameterType);
            var methodCallExpression = Expression.Call(parameterExpression, ResolveMethod, constantExpression);
            var resultExpression = Expression.Convert(methodCallExpression, parameterInfo.ParameterType);

            return resultExpression;
        }
    }
}