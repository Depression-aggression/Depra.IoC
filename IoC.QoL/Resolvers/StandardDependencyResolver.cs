// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Concurrent;
using Depra.IoC.Description;

namespace Depra.IoC.QoL.Resolvers
{
	public sealed class StandardDependencyResolver : IDependencyResolver
	{
		private readonly ISubDependencyResolver[] _subResolvers;

		public StandardDependencyResolver(ISubDependencyResolver[] subResolvers) =>
			_subResolvers = subResolvers;

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
	}
}