// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;

namespace Depra.IoC.Scope
{
	public static class ScopeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TService Resolve<TService>(this IScope self) => (TService) self.Resolve(typeof(TService));
	}
}