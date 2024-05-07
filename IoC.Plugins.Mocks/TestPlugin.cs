using Depra.IoC.QoL.Builder;

namespace Depra.IoC.Plugins.Mocks;

public sealed class TestPlugin : Plugin {
	protected override void LoadOverride(IContainerBuilder builder) {
		builder.RegisterSingleton<IServiceA, ServiceA>();
	}

	public interface IServiceA;

	public sealed class ServiceA : IServiceA;
}