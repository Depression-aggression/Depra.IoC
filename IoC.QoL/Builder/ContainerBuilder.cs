// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;
using Depra.IoC.Activation;
using Depra.IoC.Description;
using Depra.IoC.Exceptions;

namespace Depra.IoC.QoL.Builder
{
	public sealed class ContainerBuilder : IContainerBuilder
	{
		private readonly List<ServiceDescriptor> _descriptors;
		private readonly IActivationBuilder _activationBuilder;

		public ContainerBuilder(IActivationBuilder activationBuilder)
		{
			Guard.AgainstNull(activationBuilder, nameof(activationBuilder));

			_activationBuilder = activationBuilder;
			_descriptors = new List<ServiceDescriptor>();
		}

		public IContainer Build() => new Container(_activationBuilder, _descriptors);

		public void Register(ServiceDescriptor descriptor) => _descriptors.Add(descriptor);

		public override string ToString() => GetType().Name;
	}
}