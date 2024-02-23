// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Linq;
using System.Threading.Tasks;
using Depra.IoC.Scope;

namespace Depra.IoC.QoL.Scope
{
	public sealed class CombinedScope : IScope
	{
		private readonly IScope[] _scopes;
		private readonly IScope _rootScope;

		public CombinedScope(IScope rootScope, params IScope[] scopes)
		{
			_scopes = scopes;
			_rootScope = rootScope;
		}

		bool IScope.CanResolve(Type service) =>
			_rootScope.CanResolve(service) ||
			_scopes.Any(scope => scope.CanResolve(service));

		object IScope.Resolve(Type service)
		{
			if (_rootScope.CanResolve(service))
			{
				return _rootScope.Resolve(service);
			}

			foreach (var scope in _scopes)
			{
				if (scope.CanResolve(service))
				{
					return scope.Resolve(service);
				}
			}

			throw new InvalidOperationException();
		}

		void IDisposable.Dispose()
		{
			foreach (var scope in _scopes)
			{
				scope.Dispose();
			}
		}

		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			foreach (var scope in _scopes)
			{
				await scope.DisposeAsync();
			}
		}
	}
}