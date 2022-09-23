using System.Collections.Generic;
using Depra.IoC.Domain.Container;

namespace Depra.IoC.Domain.CompositeRoot
{
    public interface IInstaller
    {
        void InstallBindings();
    }

    public interface ICompositeInstaller<out TInstaller> : IInstaller where TInstaller : IInstaller
    {
        IReadOnlyList<TInstaller> LeafInstallers { get; }
    }

    public abstract class InstallerBase : IInstaller
    {
        protected IContainer Container { get; }

        public abstract void InstallBindings();
    }
}