using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Scope;

namespace Depra.IoC.Application.Activation
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}