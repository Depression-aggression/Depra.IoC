// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;
using Depra.IoC.Injection.Factory;
using Depra.IoC.Scope;

namespace Depra.IoC.Injection.Resolvers
{
	public sealed class TypedDependencyResolver : IDependencyResolver
	{
		private readonly Type[] _types;
		private readonly IInjectionFactory _factory;

		public TypedDependencyResolver(IInjectionFactory factory, params Type[] types)
		{
			_types = types;
			_factory = factory;
		}

		void IDependencyResolver.Resolve(IScope scope, object target)
		{
			const BindingFlags BINDING_ATTR = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			foreach (var field in target.GetType().GetFields(BINDING_ATTR))
			{
				if (field.IsStatic == false && Array.Exists(_types, x => x == field.FieldType))
				{
					_factory.Create(field).Invoke(target, scope.Resolve(field.FieldType));
				}
			}
		}
	}
}