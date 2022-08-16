using System;
using Depra.IoC.Descriptors.Abstract;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation.Interfaces
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}