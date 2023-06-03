using Depra.IoC.Container;
using Depra.IoC.Description;

namespace Depra.IoC.Builder
{
    public interface IContainerBuilder
    {
        IContainer Build();
        
        void Register(ServiceDescriptor descriptor);
    }
}