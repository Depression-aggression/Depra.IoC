// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;

namespace Depra.IoC.Description
{
	public sealed class MultipleServicesDescriptor : ServiceDescriptor
	{
		public MultipleServicesDescriptor(Type type, LifetimeType lifetime, ServiceDescriptor[] descriptors) :
			base(type, lifetime)
		{
			Guard.AgainstNull(descriptors, nameof(descriptors));
			Descriptors = descriptors;
		}

		public ServiceDescriptor[] Descriptors { get; }
	}
}