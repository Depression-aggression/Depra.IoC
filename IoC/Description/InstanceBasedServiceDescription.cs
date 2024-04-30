// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;

namespace Depra.IoC.Description
{
	public sealed class InstanceBasedServiceDescription : ServiceDescription
	{
		public InstanceBasedServiceDescription(Type type, object instance) : base(type, LifetimeType.SINGLETON)
		{
			Guard.AgainstNull(instance, nameof(instance));
			Instance = instance;
		}

		public object Instance { get; }
	}
}