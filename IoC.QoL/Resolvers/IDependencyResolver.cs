// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Concurrent;
using Depra.IoC.Description;

namespace Depra.IoC.QoL.Resolvers
{
	public interface IDependencyResolver
	{
		ServiceDescriptor Resolve(ConcurrentDictionary<Type, ServiceDescriptor> descriptors, Type service);
	}
}