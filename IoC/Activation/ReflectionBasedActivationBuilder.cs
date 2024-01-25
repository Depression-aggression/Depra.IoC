// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Reflection;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
	public sealed class ReflectionBasedActivationBuilder : BaseActivationBuilder
	{
		private static object[] GetConstructorArguments(IScope scope, IReadOnlyList<ParameterInfo> parameters)
		{
			if (parameters.Count == 0)
			{
				return Array.Empty<object>();
			}

			var arguments = new object[parameters.Count];
			for (var index = 0; index < parameters.Count; index++)
			{
				var argument = parameters[index];
				var argumentType = argument.ParameterType;
				arguments[index] = scope.Resolve(argumentType);
			}

			return arguments;
		}

		protected override Func<IScope, object> BuildActivation(ConstructorInfo ctor, ParameterInfo[] args) => scope =>
		{
			var argsForConstructor = GetConstructorArguments(scope, args);
			var instance = ctor.Invoke(argsForConstructor);

			return instance;
		};
	}
}