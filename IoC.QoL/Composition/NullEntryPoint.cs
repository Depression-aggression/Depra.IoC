// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Scope;

namespace Depra.IoC.QoL.Composition
{
	public sealed class NullEntryPoint : IEntryPoint
	{
		void IEntryPoint.Resolve(IScope scope) => throw new NullEntryPointException();

		private sealed class NullEntryPointException : Exception
		{
			public NullEntryPointException() : base("Null entry point is not allowed.") { }
		}
	}
}