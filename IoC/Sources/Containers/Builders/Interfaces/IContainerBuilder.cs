using Depra.IoC.Containers.Interfaces;
using Depra.IoC.Descriptors.Abstract;

namespace Depra.IoC.Containers.Builders.Interfaces
{
    public interface IContainerBuilder
    {
        void Register(ServiceDescriptor descriptor);
        
        IContainer Build();
    }
}