// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
	public sealed class LambdaBasedActivationBuilder : BaseActivationBuilder
	{
		private static readonly MethodInfo RESOLVE_METHOD = typeof(IScope).GetMethod("Resolve");

		private static UnaryExpression SelectExpressionArgs(ParameterInfo parameterInfo, Expression parameterExpression)
		{
			var constantExpression = Expression.Constant(parameterInfo.ParameterType);
			var methodCallExpression = Expression.Call(parameterExpression, RESOLVE_METHOD, constantExpression);
			var resultExpression = Expression.Convert(methodCallExpression, parameterInfo.ParameterType);

			return resultExpression;
		}

		protected override Func<IScope, object> BuildActivation(ConstructorInfo constructor, ParameterInfo[] args)
		{
			var scopeParameter = Expression.Parameter(typeof(IScope), "scope");
			var expressionArgs = args.Select(x => SelectExpressionArgs(x, scopeParameter));
			var @new = Expression.New(constructor, expressionArgs);
			var lambda = Expression.Lambda<Func<IScope, object>>(@new, scopeParameter);

			return lambda.Compile();
		}
	}
}