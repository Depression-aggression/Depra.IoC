// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Activation;
using Depra.IoC.Description;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;
using Depra.IoC.Scope;

namespace Depra.IoC.UnitTests;

internal sealed class ContainerTests
{
	private static IEnumerable<LifetimeType> GetLifetime()
	{
		yield return LifetimeType.SCOPED;
		yield return LifetimeType.TRANSIENT;
		yield return LifetimeType.SINGLETON;
	}

	private static IEnumerable<IActivationBuilder> GetActivationBuilders()
	{
		yield return new LambdaBasedActivationBuilder();
		yield return new ReflectionBasedActivationBuilder();
	}

	[Test]
	public void ResolveByImplType_WhenRegisteredByImplType_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetLifetime))] LifetimeType lifetime,
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var implementationType = typeof(Mocks.TestService);
		var descriptors = new ServiceDescriptor[]
			{ new TypeBasedServiceDescriptor(implementationType, implementationType, lifetime) };
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.TestService>();

		// Assert:
		service.Should().BeOfType(implementationType);
	}

	[Test]
	public void ResolveByInterface_WhenRegisteredByInterface_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetLifetime))] LifetimeType lifetime,
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var interfaceType = typeof(Mocks.ITestService);
		var implementationType = typeof(Mocks.TestService);
		var descriptors = new ServiceDescriptor[]
			{ new TypeBasedServiceDescriptor(implementationType, interfaceType, lifetime) };
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.ITestService>();

		// Assert:
		service.Should().BeOfType(implementationType);
	}
#if DEBUG
	[Test]
	public void ResolveByType_WhenTypeNotRegisteredInContainer_ThenInvalidOperationExceptionIsThrown(
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var descriptions = Array.Empty<ServiceDescriptor>();
		using var container = new Container(activationBuilder, descriptions);
		var scope = container.CreateScope();

		// Act:
		var act = () => scope.Resolve<Mocks.TestService>();

		// Assert:
		act.Should().Throw<UnableFindRegistration>();
	}
#endif
	[Test]
	public void ResolveType_WhenTypeConstructorIsEmpty_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		const LifetimeType LIFETIME = LifetimeType.TRANSIENT;
		var interfaceType = typeof(Mocks.ITestService);
		var implementationType = typeof(Mocks.TestServiceWithEmptyConstructor);
		var descriptors = new ServiceDescriptor[]
			{ new TypeBasedServiceDescriptor(implementationType, interfaceType, LIFETIME) };
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.ITestService>();

		// Assert:
		service.Should().BeOfType<Mocks.TestServiceWithEmptyConstructor>();
	}

	[Test]
	public void ResolveType_WhenTypeConstructorIsNotEmpty_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var descriptors = new ServiceDescriptor[]
		{
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.SINGLETON,
				type: typeof(Mocks.TestServiceWithConstructor.Token),
				implementationType: typeof(Mocks.TestServiceWithConstructor.Token)),
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.TRANSIENT,
				type: typeof(Mocks.ITestService),
				implementationType: typeof(Mocks.TestServiceWithConstructor))
		};
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.ITestService>();

		// Assert:
		service.Should().BeOfType<Mocks.TestServiceWithConstructor>();
	}

	[Test]
	public void ResolveType_WhenTypeIsGeneric_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var descriptors = new ServiceDescriptor[]
		{
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.TRANSIENT,
				type: typeof(Mocks.EmptyGeneric), implementationType: typeof(Mocks.EmptyGeneric)),
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.TRANSIENT,
				type: typeof(Mocks.GenericTestService<Mocks.EmptyGeneric>),
				implementationType: typeof(Mocks.GenericTestService<Mocks.EmptyGeneric>))
		};
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.GenericTestService<Mocks.EmptyGeneric>>();

		// Assert:
		service.Should().BeOfType<Mocks.GenericTestService<Mocks.EmptyGeneric>>();
	}

	[Test]
	public void ResolveType_WhenTypeIsEnumerable_ThenResolvedTypeEqualsToRegisteredType(
		[ValueSource(nameof(GetActivationBuilders))]
		IActivationBuilder activationBuilder)
	{
		// Arrange:
		var descriptors = new ServiceDescriptor[]
		{
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.TRANSIENT,
				type: typeof(Mocks.EmptyGeneric), implementationType: typeof(Mocks.EmptyGeneric)),
			new TypeBasedServiceDescriptor(lifetime: LifetimeType.TRANSIENT,
				type: typeof(Mocks.EnumerableTestService), implementationType: typeof(Mocks.EnumerableTestService))
		};
		using var container = new Container(activationBuilder, descriptors);
		var scope = container.CreateScope();

		// Act:
		var service = scope.Resolve<Mocks.EnumerableTestService>();

		// Assert:
		service.Should().BeOfType<Mocks.EnumerableTestService>();
	}
}