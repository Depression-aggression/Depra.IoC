// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;

namespace Depra.IoC.Exceptions
{
	internal sealed class SuitableConstructorNotFound : Exception
	{
		public SuitableConstructorNotFound(MemberInfo type) :
			base($"No suitable constructor for '{type.Name}' found!") { }
	}
}