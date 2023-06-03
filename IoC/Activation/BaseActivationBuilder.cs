using System;
using System.Linq;
using System.Reflection;
using Depra.IoC.Description;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
    public abstract class BaseActivationBuilder : IActivationBuilder
    {
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.Instance;

        public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
        {
            var typeBased = (TypeBasedServiceDescriptor)descriptor;
            var constructor = SelectConstructor(typeBased.ImplementationType);
            if (constructor == null)
            {
                throw new InvalidOperationException("No suitable constructor found!");
            }

            var args = constructor.GetParameters();

            return BuildActivationInternal(constructor, args);
        }

        protected abstract Func<IScope, object> BuildActivationInternal(
            ConstructorInfo constructor,
            ParameterInfo[] args);

        private static ConstructorInfo SelectConstructor(Type implementation) =>
            implementation.GetConstructors(FLAGS).Single();

        public override string ToString() => GetType().Name;
    }
}