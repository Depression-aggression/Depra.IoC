// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;
using Depra.IoC.Exceptions;
using Depra.IoC.QoL.Builder;

namespace Depra.IoC.Plugins;

public static class ContainerBuilderExtensions {
	public static void RegisterAssemblyPlugins(this IContainerBuilder self, Assembly assembly) {
		foreach (var definedType in assembly.DefinedTypes) {
			if (definedType.BaseType != typeof(Plugin)) {
				continue;
			}

			var pluginInstance = (Plugin)Activator.CreateInstance(definedType);
			Guard.AgainstNull(pluginInstance, nameof(pluginInstance));
			pluginInstance!.Load(self);
		}
	}
}