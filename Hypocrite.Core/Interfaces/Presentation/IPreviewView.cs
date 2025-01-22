namespace Hypocrite.Core.Interfaces.Presentation
{
    /// <summary>
    /// Only for non desktop
    /// </summary>
    public interface IPreviewView : IBaseView
    {
        void CallPreviewDoneEvent();
    }
}
