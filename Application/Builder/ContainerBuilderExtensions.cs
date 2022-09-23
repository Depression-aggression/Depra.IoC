using System;
using Depra.IoC.Application.Descriptors;
using Depra.IoC.Domain.Builder;
using Depra.IoC.Domain.Enums;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Builder
{
    public static class ContainerBuilderExtensions
    {
        public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, LifetimeType.Transient);

        public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, LifetimeType.Scoped);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
            Type implementation)
            => builder.RegisterType(service, implementation, LifetimeType.Singleton);

        public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, LifetimeType.Transient);

        public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, LifetimeType.Scoped);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory)
            => builder.RegisterFactory(service, factory, LifetimeType.Singleton);

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service, object instance)
            => builder.RegisterInstance(service, instance);

        public static IContainerBuilder RegisterTransient<TImplementation>(this IContainerBuilder builder)
            => builder.RegisterType(typeof(TImplementation), typeof(TImplementation), LifetimeType.Transient);

        public static IContainerBuilder RegisterScoped<TImplementation>(this IContainerBuilder builder)
            => builder.RegisterType(typeof(TImplementation), typeof(TImplementation), LifetimeType.Scoped);

        public static IContainerBuilder RegisterSingleton<TImplementation>(this IContainerBuilder builder) =>
            builder.RegisterType(typeof(TImplementation), typeof(TImplementation), LifetimeType.Singleton);
        
        public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifetimeType.Transient);

        public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifetimeType.Scoped);

        public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder)
            where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifetimeType.Singleton);

        public static IContainerBuilder RegisterType(this IContainerBuilder builder, Type service, Type implementation,
            LifetimeType lifetime)
        {
            builder.Register(new TypeBasedServiceDescriptor(implementation, service, lifetime));
            return builder;
        }

        private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type service,
            Func<IScope, object> factory, LifetimeType lifetime)
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