using Depra.IoC.Activation;
using Depra.IoC.Plugins.Mocks;
using FluentAssertions;

namespace Depra.IoC.Plugins.UnitTests;

public sealed class PluginConfigurationTests {
	private readonly PluginConfiguration _configuration = new();
	private readonly IActivationBuilder _activation = new LambdaBasedActivationBuilder();

	[Fact]
	public void Build_ShouldReturnsNotNullContainer() {
		// Arrange & Act:
		var container = _configuration.Build(_activation);

		// Assert:
		container.Should().NotBeNull();
	}

	[Fact]
	public void Build_ShouldReturnsContainerWithTestServices() {
		// Arrange:
		var serviceType = typeof(TestPlugin.IServiceA);

		// Act:
		var container = _configuration.Build(_activation);

		// Assert:
		container.CreateScope().Resolve(serviceType).Should().NotBeNull();
	}
}