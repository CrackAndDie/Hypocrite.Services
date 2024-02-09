namespace Hypocrite.Core.Interfaces
{
    public interface IWindowProgressService
    {
        bool IsDone { get; }
        void AddWaiter();
        bool RemoveWaiter();
    }
}
