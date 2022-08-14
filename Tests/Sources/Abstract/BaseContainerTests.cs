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
        public void Resolve_Singleton()
        {
            var scope = Builder.RegisterSingleton<IServiceTest, Service>().Build().CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Transient()
        {
            var scope = Builder.RegisterTransient<IServiceTest, Service>().Build().CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Scoped()
        {
            var scope = Builder.RegisterScoped<IServiceTest, Service>().Build().CreateScope();
            var service = scope.Resolve<IServiceTest>();

            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void Resolve_Non_Registered()
        {
            var scope = Builder.Build().CreateScope();

            // Expected exception.
            Assert.That(() => scope.Resolve<Service>(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Resolve_Instance_With_Empty_Constructor()
        {
            var scope = Builder
                .RegisterTransient<IServiceTest, ServiceWithEmptyConstructor>()
                .Build()
                .CreateScope();

            var service = scope.Resolve<IServiceTest>();
            
            Assert.IsNotNull(service);
            Assert.IsTrue(service.IsCreated);
        }
        
        [Test]
        public void Resolve_Instance_With_Constructor()
        {
            var scope = Builder
                .RegisterSingleton<ServiceWithConstructor.Token>()
                .RegisterTransient<IServiceTest, ServiceWithConstructor>()
                .Build()
                .CreateScope();

            var service = scope.Resolve<IServiceTest>();
            
            Assert.IsNotNull(service);
            Assert.IsTrue(service.IsCreated);
        }
    }
}