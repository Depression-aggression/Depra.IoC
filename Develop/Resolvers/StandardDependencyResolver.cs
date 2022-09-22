using System;
using System.Collections.Concurrent;
using Depra.IoC.Domain.Description;

namespace Develop.Resolvers
{
    public class StandardDependencyResolver : IDependencyResolver
    {
        private readonly ISubDependencyResolver[] _subResolvers;

        public ServiceDescriptor Resolve(ConcurrentDictionary<Type, ServiceDescriptor> descriptors, Type service)
        {
            if (descriptors.TryGetValue(service, out var descriptor))
            {
                return descriptor;
            }

            foreach (var subResolver in _subResolvers)
            {
                var result = subResolver.Resolve(descriptors, service);
                if (result != null)
                {
                    return result;
                }
            }

            throw new InvalidOperationException();
        }
        
        public StandardDependencyResolver(ISubDependencyResolver[] subResolvers)
        {
            _subResolvers = subResolvers;
        }
    }
}