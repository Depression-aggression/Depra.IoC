// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Reflection;
using Depra.IoC.Injection.Delegates;

namespace Depra.IoC.Injection.Factory
{
	public sealed class ReflectionBasedInjectorFactory : IInjectionFactory
	{
		FieldInjector IInjectionFactory.Create(FieldInfo field) => field.SetValue;
		PropertyInjector IInjectionFactory.Create(PropertyInfo property) => property.SetValue;
		ConstructorInjector IInjectionFactory.Create(ConstructorInfo constructor) => constructor.Invoke;
		MethodInjector IInjectionFactory.Create(MethodInfo method) => (target, args) => method.Invoke(target, args);
	}
}