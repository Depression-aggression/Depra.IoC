using Depra.IoC.Activation;
using Depra.IoC.Builder;
using Depra.IoC.Container;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;
using Depra.IoC.Extensions;
using Depra.IoC.UnitTests.Services;

namespace Depra.IoC.UnitTests;

[TestFixture]
internal sealed class ContainerTests
{
    private static IEnumerable<LifetimeType> GetLifetime()
    {
        yield return LifetimeType.Scoped;
        yield return LifetimeType.Transient;
        yield return LifetimeType.Singleton;
    }

    private static IEnumerable<IActivationBuilder> GetActivationBuilders()
    {
        yield return new LambdaBasedActivationBuilder();
        yield return new ReflectionBasedActivationBuilder();
    }

    [Test]
    public void WhenRegisteringByImplType_AndResolvingByImplType_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetLifetime))] LifetimeType lifetime,
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        var implementationType = typeof(TestService);
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterType(implementationType, implementationType, lifetime)
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<TestService>();

        // Assert.
        service.Should().BeOfType(implementationType);
    }

    [Test]
    public void WhenRegisteringByInterface_AndResolvingByInterface_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetLifetime))] LifetimeType lifetime,
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        var interfaceType = typeof(ITestService);
        var implementationType = typeof(TestService);
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterType(interfaceType, implementationType, lifetime)
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<ITestService>();

        // Assert.
        service.Should().BeOfType(implementationType);
    }

    [Test]
    public void WhenResolvingByType_AndTypeNotRegisteredInContainer_ThenInvalidOperationExceptionIsThrown(
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        using var container = new ContainerBuilder(activationBuilder).Build();
        var scope = container.CreateScope();

        // Act.
        var act = () => scope.Resolve<TestService>();

        // Assert.
        act.Should().Throw<UnableFindRegistrationException>();
    }

    [Test]
    public void WhenResolvingType_AndTypeConstructorIsEmpty_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterTransient<ITestService, TestServiceWithEmptyConstructor>()
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<ITestService>();

        // Assert.
        service.Should().BeOfType<TestServiceWithEmptyConstructor>();
    }

    [Test]
    public void WhenResolvingType_AndTypeConstructorIsNotEmpty_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterSingleton<TestServiceWithConstructor.Token>()
            .RegisterTransient<ITestService, TestServiceWithConstructor>()
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<ITestService>();

        // Assert.
        service.Should().BeOfType<TestServiceWithConstructor>();
    }

    [Test]
    public void WhenResolvingType_AndTypeIsGeneric_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterTransient<EmptyGeneric>()
            .RegisterTransient<GenericTestService<EmptyGeneric>>()
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<GenericTestService<EmptyGeneric>>();

        // Assert.
        service.Should().BeOfType<GenericTestService<EmptyGeneric>>();
    }

    [Test]
    public void WhenResolvingType_AndTypeIsEnumerable_ThenResolvedTypeEqualsToRegisteredType(
        [ValueSource(nameof(GetActivationBuilders))]
        IActivationBuilder activationBuilder)
    {
        // Arrange.
        using var container = new ContainerBuilder(activationBuilder)
            .RegisterTransient<EmptyGeneric>()
            .RegisterTransient<EnumerableTestService>()
            .Build();
        var scope = container.CreateScope();

        // Act.
        var service = scope.Resolve<EnumerableTestService>();

        // Assert.
        service.Should().BeOfType<EnumerableTestService>();
    }
}