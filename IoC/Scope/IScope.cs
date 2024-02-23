// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.IoC.Scope
{
	public interface IScope : IDisposable, IAsyncDisposable
	{
		bool CanResolve(Type service);

		object Resolve(Type service);
	}
}