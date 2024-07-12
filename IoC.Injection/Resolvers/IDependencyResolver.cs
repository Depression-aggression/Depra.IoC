// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.IoC.Scope;

namespace Depra.IoC.Injection.Resolvers
{
	public interface IDependencyResolver
	{
		void Resolve(IScope scope, object target);
	}
}