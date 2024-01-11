// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

namespace Depra.IoC.Composition
{
	public interface ICompositeRoot
	{
		IContainer Container { get; }

		void Install();

		void Uninstall();
	}
}