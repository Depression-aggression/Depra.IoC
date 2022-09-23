using System;
using System.Collections.Concurrent;
using Depra.IoC.Domain.Description;

namespace Develop.Resolvers
{
    public interface ISubDependencyResolver
    {
        ServiceDescriptor Resolve(ConcurrentDictionary<Type, ServiceDescriptor> descriptors, Type service);
    }
}