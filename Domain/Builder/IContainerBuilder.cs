using Depra.IoC.Domain.Container;
using Depra.IoC.Domain.Description;

namespace Depra.IoC.Domain.Builder
{
    public interface IContainerBuilder
    {
        IContainer Build();
        
        void Register(ServiceDescriptor descriptor);
    }
}