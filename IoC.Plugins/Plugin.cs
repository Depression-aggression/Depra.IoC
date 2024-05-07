// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Exceptions;
using Depra.IoC.QoL.Builder;

namespace Depra.IoC.Plugins;

public abstract class Plugin {
	internal void Load(IContainerBuilder containerBuilder) {
		Guard.AgainstNull(containerBuilder, nameof(containerBuilder));
		LoadOverride(containerBuilder);
	}

	protected abstract void LoadOverride(IContainerBuilder builder);
}