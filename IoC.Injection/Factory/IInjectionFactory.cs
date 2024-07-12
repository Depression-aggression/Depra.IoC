﻿// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Reflection;
using Depra.IoC.Injection.Delegates;

namespace Depra.IoC.Injection.Factory
{
	public interface IInjectionFactory
	{
		FieldInjector Create(FieldInfo field);
		MethodInjector Create(MethodInfo method);
		PropertyInjector Create(PropertyInfo property);
		ConstructorInjector Create(ConstructorInfo constructor);
	}
}