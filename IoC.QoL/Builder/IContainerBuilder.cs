// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Description;

namespace Depra.IoC.QoL.Builder
{
	public interface IContainerBuilder
	{
		IContainer Build();

		void Register(ServiceDescriptor descriptor);
	}
}