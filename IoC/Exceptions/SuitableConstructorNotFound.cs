// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.IoC.Exceptions
{
	internal sealed class SuitableConstructorNotFound : Exception
	{
		public SuitableConstructorNotFound() : base("No suitable constructor found!") { }
	}
}