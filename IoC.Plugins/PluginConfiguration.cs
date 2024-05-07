// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Depra.IoC.Activation;
using Depra.IoC.QoL.Builder;
using Mono.Cecil;

namespace Depra.IoC.Plugins;

public sealed class PluginConfiguration {
	private readonly IEnumerable<Type> _filter;
	private readonly DirectoryInfo _pluginsLocation;

	public PluginConfiguration(DirectoryInfo location = null) : this(location, [typeof(PluginAttribute)]) { }

	public PluginConfiguration(DirectoryInfo location = null, params Type[] filter) : this(filter, location) { }

	public PluginConfiguration(IEnumerable<Type> filter, DirectoryInfo location = null) {
		_filter = filter;
		_pluginsLocation = location ?? new FileInfo(GetType().Assembly.Location).Directory;
	}

	public IContainer Build(IActivationBuilder activation) {
		var builder = new ContainerBuilder(activation);
		LoadPlugins(_pluginsLocation, builder);

		return builder.Build();
	}

	private void LoadPlugins(DirectoryInfo directory, IContainerBuilder builder) {
		var dlls = directory!.GetFiles("*.dll");
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (var dll in dlls.Where(x => IsSuitable(x.FullName))) {
			var loadedAssembly = LoadAssembly(dll, assemblies);
			builder.RegisterAssemblyModules(loadedAssembly);
		}
	}

	private Assembly LoadAssembly(FileSystemInfo dll, IEnumerable<Assembly> assemblies) {
		var defaultContext = AssemblyLoadContext.Default;
		var loadedAssembly = assemblies.FirstOrDefault(x =>
			x.Location.Equals(dll.FullName, StringComparison.InvariantCultureIgnoreCase));

		if (loadedAssembly == null) {
			loadedAssembly = defaultContext.LoadFromAssemblyPath(dll.FullName);
		}

		return loadedAssembly;
	}

	private bool IsSuitable(string path) => AssemblyDefinition.ReadAssembly(path)
		.CustomAttributes
		.Any(customAttribute => _filter.Any(x =>
			customAttribute.AttributeType.Name == x.Name &&
			customAttribute.AttributeType.Namespace == x.Namespace));
}