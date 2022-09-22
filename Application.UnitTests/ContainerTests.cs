using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Depra.IoC.Application.Containers.Builders.Extensions;
using Depra.IoC.Application.Containers.Builders.Interfaces;
using Depra.IoC.Application.UnitTests.Services;
using Depra.IoC.Containers.Builders.Impl;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Scope;
using FluentAssertions;
using NUnit.Framework;

namespace Depra.IoC.Application.UnitTests
{
    [TestFixture]
    public class ContainerTests
    {
        private static IEnumerable<IContainerBuilder> GetContainers()
        {
            yield return new LambdaBasedContainerBuilder();
            yield return new ReflectionBasedContainerBuilder();
        }

        [Test]
        public void WhenResolveByType_AndLifetimeIsSingleton_ThenCanResolve(
            [Values(LifetimeType.Scoped, LifetimeType.Transient, LifetimeType.Singleton)]
            LifetimeType lifetime,
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
                .RegisterType(typeof(TestService), typeof(TestService), lifetime)
                .Build();
            var scope = container.CreateScope();

            // Act.
            var service = scope.Resolve<TestService>();

            // Assert.
            service.Should().BeOfType<TestService>();
        }

        [Test]
        public void WhenResolveByInterface_AndLifetimeIsSingleton_ThenCanResolve(
            [Values(LifetimeType.Scoped, LifetimeType.Transient, LifetimeType.Singleton)]
            LifetimeType lifetime,
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
                .RegisterType(typeof(ITestService), typeof(TestService), lifetime)
                .Build();
            var scope = container.CreateScope();

            // Act.
            var service = scope.Resolve<ITestService>();

            // Assert.
            service.Should().BeOfType<TestService>();
        }

        [Test]
        public void WhenResolveByType_AndTypeNotRegisteredInContainer_ThenThrowInvalidOperationException(
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder.Build();
            var scope = container.CreateScope();

            // Act.
            void ResolveService() => scope.Resolve<TestService>();

            // Assert.
            Assert.That(ResolveService, Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void WhenResolveType_AndTypeConstructorIsEmpty_ThenResolvedServiceNotNull(
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
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
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
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
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
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
            [ValueSource(nameof(GetContainers))] IContainerBuilder builder)
        {
            // Arrange.
            using var container = builder
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