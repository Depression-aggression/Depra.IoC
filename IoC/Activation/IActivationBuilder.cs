using System;
using Depra.IoC.Description;
using Depra.IoC.Scope;

namespace Depra.IoC.Activation
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}