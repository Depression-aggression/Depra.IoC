using System;
using Depra.IoC.Containers.Builders.Interfaces;
using Depra.IoC.Descriptors.Impl;
using Depra.IoC.Scope;
using Depra.IoC.Structs;

namespace Depra.IoC.Containers.Builders.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, Lifetime.Transient);

        public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, Lifetime.Scoped);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, Lifetime.Singleton);

        public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, Lifetime.Transient);

        public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, Lifetime.Scoped);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, Lifetime.Singleton);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service, object instance)
            => builder.RegisterInstance(service, instance);

        public static IContainerBuilder RegisterTransient<TImplementation>(this IContainerBuilder builder)
            => builder.RegisterType(typeof(TImplementation), typeof(TImplementation), Lifetime.Transient);

        public static IContainerBuilder RegisterScoped<TImplementation>(this IContainerBuilder builder)
            => builder.RegisterType(typeof(TImplementation), typeof(TImplementation), Lifetime.Scoped);

        public static IContainerBuilder RegisterSingleton<TImplementation>(this IContainerBuilder builder) =>
            builder.RegisterType(typeof(TImplementation), typeof(TImplementation), Lifetime.Singleton);
        
        public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), Lifetime.Transient);

        public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), Lifetime.Scoped);

        public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), Lifetime.Singleton);

        private static IContainerBuilder RegisterType(this IContainerBuilder builder, Type service, Type implementation,
            Lifetime lifetime)
        {
            builder.Register(new TypeBasedServiceDescriptor(implementation, service, lifetime));
            return builder;
        }

        private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory, Lifetime lifetime)
        {
            builder.Register(new FactoryBasedServiceDescriptor(service, lifetime, factory));
            return builder;
        }

        private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type service, object instance)
        {
            builder.Register(new InstanceBasedServiceDescriptor(service, instance));
            return builder;
        }
    }
}