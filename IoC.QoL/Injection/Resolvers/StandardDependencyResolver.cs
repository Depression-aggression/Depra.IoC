using System;
using System.Reflection;
using Depra.IoC.QoL.Attributes;
using Depra.IoC.QoL.Injection.Factory;
using Depra.IoC.Scope;

namespace Depra.IoC.QoL.Injection.Resolvers
{
	public sealed class StandardDependencyResolver : IDependencyResolver
	{
		private static readonly Type DEPENDENCY_TYPE = typeof(DependencyAttribute);

		private readonly IInjectionFactory _injectionFactory;

		public StandardDependencyResolver(IInjectionFactory injectionFactory)
		{
			_injectionFactory = injectionFactory;
		}

		public void Resolve(IScope scope, object target)
		{
			var fields = target.GetType()
				.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (var field in fields)
			{
				if (field.IsStatic)
				{
					continue;
				}

				if (Attribute.IsDefined(field, DEPENDENCY_TYPE) == false)
				{
					return;
				}

				var service = scope.Resolve(field.FieldType);
				_injectionFactory.Create(field).Invoke(target, service);
			}
		}
	}
}