// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;
using Depra.IoC.Injection.Attributes;
using Depra.IoC.Injection.Factory;
using Depra.IoC.Scope;

namespace Depra.IoC.Injection.Resolvers
{
	public sealed class StandardDependencyResolver : IDependencyResolver
	{
		private readonly IInjectionFactory _factory;

		public StandardDependencyResolver(IInjectionFactory factory) => _factory = factory;

		void IDependencyResolver.Resolve(IScope scope, object target)
		{
			const BindingFlags BINDING_ATTR = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			foreach (var field in target.GetType().GetFields(BINDING_ATTR))
			{
				if (field.IsStatic == false && Attribute.IsDefined(field, typeof(DependencyAttribute)))
				{
					_factory.Create(field).Invoke(target, scope.Resolve(field.FieldType));
				}
			}
		}
	}
}