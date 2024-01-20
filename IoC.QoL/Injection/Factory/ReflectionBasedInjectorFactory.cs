// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Reflection;
using Depra.IoC.QoL.Injection.Delegates;

namespace Depra.IoC.QoL.Injection.Factory
{
	public sealed class ReflectionBasedInjectorFactory : IInjectionFactory
	{
		public ConstructorInjector Create(ConstructorInfo constructor) => constructor.Invoke;

		public PropertyInjector Create(PropertyInfo property) =>
			(target, value) => property.SetValue(target, value, null);

		public MethodInjector Create(MethodInfo method) =>
			(target, args) => method.Invoke(target, args);
	}
}