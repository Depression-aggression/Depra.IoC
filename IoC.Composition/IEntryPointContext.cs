// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;

namespace Depra.IoC.Composition
{
	public interface IEntryPointContext
	{
		IReadOnlyCollection<ILifetimeScope> LifetimeScopes { get; }
	}
}