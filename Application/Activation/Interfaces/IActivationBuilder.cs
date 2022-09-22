using System;
using Depra.IoC.Domain.Description;
using Depra.IoC.Domain.Scope;
using Depra.IoC.Scope;

namespace Depra.IoC.Application.Activation.Interfaces
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}