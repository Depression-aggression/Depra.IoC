// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Linq;
using System.Reflection;
using Depra.IoC.Description;
using Depra.IoC.Exceptions;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
	public abstract class BaseActivationBuilder : IActivationBuilder
	{
		public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
		{
			var typeBased = (TypeBasedServiceDescriptor) descriptor;
			var constructor = typeBased.ImplementationType
				.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
				.Single();

			Guard.AgainstNull(constructor, () => new SuitableConstructorNotFound());

			var args = constructor.GetParameters();

			return BuildActivation(constructor, args);
		}

		protected abstract Func<IScope, object> BuildActivation(ConstructorInfo constructor, ParameterInfo[] args);

		public override string ToString() => GetType().Name;
	}
}