// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

namespace Depra.IoC.Injection.Delegates
{
	public delegate void MethodInjector(object target, params object[] args);
}