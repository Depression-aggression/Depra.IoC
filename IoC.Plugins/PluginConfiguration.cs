// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Depra.IoC.Activation;
using Depra.IoC.QoL.Builder;
using Mono.Cecil;

namespace Depra.IoC.Plugins;

public sealed class PluginConfiguration {
	private readonly IEnumerable<Type> _filter;

	public PluginConfiguration() : this([typeof(PluginAttribute)]) { }

	public PluginConfiguration(params Type[] filter) => _filter = filter;

	public PluginConfiguration(IEnumerable<Type> filter) => _filter = filter;

	public IContainer Build(IActivationBuilder activation) {
		var builder = new ContainerBuilder(activation);
		LoadPlugins(builder);

		return builder.Build();
	}

	private void LoadPlugins(IContainerBuilder builder) {
		var directory = new FileInfo(GetType().Assembly.Location).Directory;
		var dlls = directory!.GetFiles("*.dll");
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();

		foreach (var dll in dlls.Where(x => IsSuitable(x.FullName))) {
			var defaultContext = AssemblyLoadContext.Default;
			var loadedAssembly = assemblies.FirstOrDefault(x =>
				x.Location.Equals(dll.FullName, StringComparison.InvariantCultureIgnoreCase));

			if (loadedAssembly == null) {
				loadedAssembly = defaultContext.LoadFromAssemblyPath(dll.FullName);
			}

			builder.RegisterAssemblyModules(loadedAssembly);
		}
	}

	private bool IsSuitable(string path) => AssemblyDefinition.ReadAssembly(path)
		.CustomAttributes
		.Any(customAttribute => _filter.Any(x =>
			customAttribute.AttributeType.Name == x.Name &&
			customAttribute.AttributeType.Namespace == x.Namespace));
}