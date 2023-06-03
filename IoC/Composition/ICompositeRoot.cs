using Depra.IoC.Container;

namespace Depra.IoC.Composition
{
    public interface ICompositeRoot
    {
        IContainer Container { get; }
        
        void Install();
        
        void Uninstall();
    }
}