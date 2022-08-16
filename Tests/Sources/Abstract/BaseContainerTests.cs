using System;
using Depra.IoC.Containers.Builders.Extensions;
using Depra.IoC.Containers.Builders.Interfaces;
using Depra.IoC.Scope;
using NUnit.Framework;

namespace Depra.IoC.Tests.Abstract
{
    public abstract class BaseContainerTests
    {
        protected IContainerBuilder Builder { get; set; }

        public abstract void SetUp();

        [Test]
        public void Resolve_Singleton_Type()
        {
            using var container = Builder.RegisterSingleton<Service>().Build();
            var scope = container.CreateScope();
            var service = scope.Resolve<Service>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Singleton_Type_As_Interface()
        {
            using var container = Builder.RegisterSingleton<IServiceTest, Service>().Build();
            var scope = container.CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Transient_Type_As_Interface()
        {
            using var container = Builder.RegisterTransient<IServiceTest, Service>().Build();
            var scope = container.CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Scoped_Type_As_Interface()
        {
            using var container = Builder.RegisterScoped<IServiceTest, Service>().Build();
            var scope = container.CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Non_Registered_Type()
        {
            using var container = Builder.Build();
            var scope = container.CreateScope();

            // Expected exception.
            Assert.That(() => scope.Resolve<Service>(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ResolveType_With_Empty_Constructor()
        {
            using var container = Builder
                .RegisterTransient<IServiceTest, ServiceWithEmptyConstructor>()
                .Build();

            var scope = container.CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void Resolve_Type_With_Constructor()
        {
            using var container = Builder
                .RegisterSingleton<ServiceWithConstructor.Token>()
                .RegisterTransient<IServiceTest, ServiceWithConstructor>()
                .Build();

            var scope = container.CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void Resolve_Generic_Type()
        {
            using var container = Builder
                .RegisterTransient<EmptyGeneric>()
                .RegisterTransient<GenericService<EmptyGeneric>>()
                .Build();

            var scope = container.CreateScope();
            var service = scope.Resolve<GenericService<EmptyGeneric>>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void Resolve_Enumerable_Type()
        {
            using var container = Builder
                .RegisterTransient<EmptyGeneric>()
                .RegisterTransient<EnumerableService>()
                .Build();

            var scope = container.CreateScope();
            var service = scope.Resolve<EnumerableService>();

            Assert.IsNotNull(service);
        }
    }
}