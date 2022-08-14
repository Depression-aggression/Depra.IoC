using System;
using Depra.IoC.Descriptors.Abstract;

namespace Depra.IoC.Scope.Activation.Interfaces
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}