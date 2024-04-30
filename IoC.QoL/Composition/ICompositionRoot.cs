// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Scope;

namespace Depra.IoC.QoL.Composition
{
	public interface ICompositionRoot
	{
		void Register();

		void Resolve(IScope scope);

		void Release();
	}
}