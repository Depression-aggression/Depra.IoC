// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Scope;

namespace Depra.IoC.QoL.Composition
{
	public sealed class NullCompositionRoot : ICompositionRoot
	{
		void ICompositionRoot.Register() => throw new NullCompositionRootException();

		void ICompositionRoot.Resolve(IScope scope) => throw new NullCompositionRootException();

		void ICompositionRoot.Release() => throw new NullCompositionRootException();

		private sealed class NullCompositionRootException : Exception
		{
			public NullCompositionRootException() : base("Null composition root is not allowed.") { }
		}
	}
}