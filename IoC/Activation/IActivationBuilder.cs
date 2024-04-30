// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.IoC.Description;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
	public interface IActivationBuilder
	{
		Func<IScope, object> BuildActivation(ServiceDescription description);
	}
}