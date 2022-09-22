using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Description;

namespace Depra.IoC.Application.Containers.Builders.Interfaces
{
    public interface IContainerBuilder
    {
        void Register(ServiceDescriptor descriptor);
        
        IContainer Build();
    }
}