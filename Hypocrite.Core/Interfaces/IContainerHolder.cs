using Prism.Ioc;

namespace Hypocrite.Core.Interfaces
{
    public interface IContainerHolder
    {
        IContainerProvider Container { get; }
    }
}
