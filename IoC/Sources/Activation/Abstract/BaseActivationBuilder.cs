using System;
using System.Linq;
using System.Reflection;
using Depra.IoC.Activation.Interfaces;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Descriptors.Impl;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation.Abstract
{
    public abstract class BaseActivationBuilder : IActivationBuilder
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

        public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
        {
            var typeBased = (TypeBasedServiceDescriptor)descriptor;
            var constructor = SelectConstructor(typeBased.ImplementationType);
            if (constructor == null)
            {
                throw new InvalidOperationException();
            }
            
            var args = constructor.GetParameters();

            return BuildActivationInternal(constructor, args);
        }

        protected abstract Func<IScope, object> BuildActivationInternal(ConstructorInfo constructor, ParameterInfo[] args);

        private static ConstructorInfo SelectConstructor(Type implementation) =>
            implementation.GetConstructors(Flags).Single();
    }
}