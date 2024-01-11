// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Enums;
using Depra.IoC.Exceptions;
using Depra.IoC.Scope;

namespace Depra.IoC.Description
{
	public sealed class FactoryBasedServiceDescriptor : ServiceDescriptor
	{
		public FactoryBasedServiceDescriptor(Type type, LifetimeType lifetime, Func<IScope, object> factory) :
			base(type, lifetime)
		{
			Guard.AgainstNull(factory, nameof(factory));
			Factory = factory;
		}

		public Func<IScope, object> Factory { get; }
	}
}