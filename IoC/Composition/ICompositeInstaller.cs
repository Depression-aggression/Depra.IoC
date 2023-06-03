using System.Collections.Generic;

namespace Depra.IoC.Composition
{
    public interface ICompositeInstaller<out TInstaller> : IInstaller where TInstaller : IInstaller
    {
        IReadOnlyList<TInstaller> LeafInstallers { get; }
    }
}