using System;
using System.Collections.Concurrent;
using Depra.IoC.Description;

namespace Develop.Resolvers
{
    public interface IDependencyResolver
    {
        ServiceDescriptor Resolve(ConcurrentDictionary<Type, ServiceDescriptor> descriptors, Type service);
    }
}