// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Scope;

namespace Depra.IoC.Composition
{
	public interface ICompositionRoot
	{
		void Compose(IScope scope);
	}
}