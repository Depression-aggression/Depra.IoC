// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.QoL.Builder;

namespace Depra.IoC.Plugins.Mocks;

public sealed class StubPlugin : Plugin {
	protected override void Configure(IContainerBuilder builder) {
		builder.RegisterSingleton<IServiceA, ServiceA>();
	}

	public interface IServiceA;

	public sealed class ServiceA : IServiceA;
}