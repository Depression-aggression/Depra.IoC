using System;
using System.Collections.Generic;
using Depra.IoC.Application.Activation;
using Depra.IoC.Application.Builder;
using Depra.IoC.Application.UnitTests.Services;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Domain.Extensions;
using Depra.IoC.Infrastructure.Activation;
using FluentAssertions;
using NUnit.Framework;

namespace Depra.IoC.Application.UnitTests
{
    [TestFixture]
    public class ContainerTests
    {
        private static IEnumerable<IActivationBuilder> GetActivationBuilders()
        {
            yield return new LambdaBasedActivationBuilder();
            yield return new ReflectionBasedActivationBuilder();
        }

        [Test]
        public void WhenRegisterByImplType_AndResolveByImplType_ThenCanResolve(
            [Values(LifetimeType.Scoped, LifetimeType.Transient, LifetimeType.Singleton)]
            LifetimeType lifetime,
            [ValueSource(nameof(GetActivationBuilders))]
            IActivationBuilder activationBuilder)
        {
            // Arrange.
            var implType = typeof(TestService);
            using var container = new ContainerBuilder(activationBuilder)
                .RegisterType(implType, implType, lifetime)
                .Build();
            var scope = container.CreateScope();

            // Act.
            var service = scope.Resolve<TestService>();

            // Assert.
            service.Should().BeOfType<TestService>();
        }

        [Test]
        public void WhenRegisteredByInterface_AndResolveByInterface_ThenCanResolve(
            [Values(LifetimeType.Scoped, LifetimeType.Transient, LifetimeType.Singleton)]
            LifetimeType lifetime,
            [ValueSource(nameof(GetActivationBuilders))]
            IActivationBuilder activationBuilder)
        {
            // Arrange.
            var interfaceType = typeof(ITestService);
            var implType = typeof(TestService);
            using var container = new ContainerBuilder(activationBuilder)
                .RegisterType(interfaceType, implType, lifetime)
                .Build();
            var scope = container.CreateScope();

            // Act.
            var service = scope.Resolve<ITestService>();

            // Assert.
            service.Should().BeOfType<TestService>();
        }

        [Test]
        public void WhenResolveByType_AndTypeNotRegisteredInContainer_ThenThrowInvalidOperationException(
            [ValueSource(nameof(GetActivationBuilders))]
            IActivationBuilder activationBuilder)
        {
            // Arrange.
            using var container = new ContainerBuilder(activationBuilder).Build();
            var scope = container.CreateScope();

            // Act.
            void ResolveService() => scope.Resolve<TestService>();

            // Assert.
            Assert.That(ResolveService, Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void WhenResolveType_AndTypeConstructorIsEmpty_ThenResolvedServiceNotNull(
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
            service.Should().NotBeNull();
        }

        [Test]
        public void WhenResolveType_AndTypeConstructorIsNotEmpty_ThenResolvedServiceNotNull(
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
            service.Should().NotBeNull();
        }

        [Test]
        public void WhenResolveType_AndTypeIsGeneric_ThenResolvedServiceNotNull(
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
            service.Should().NotBeNull();
        }

        [Test]
        public void WhenResolveType_AndTypeIsEnumerable_ThenResolvedServiceNotNull(
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
            service.Should().NotBeNull();
        }
    }
}