// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Reflection;
using Depra.IoC.QoL.Injection.Delegates;

namespace Depra.IoC.QoL.Injection.Factory
{
	public interface IInjectionFactory
	{
		ConstructorInjector Create(ConstructorInfo constructor);

		PropertyInjector Create(PropertyInfo property);

		MethodInjector Create(MethodInfo method);
	}
}