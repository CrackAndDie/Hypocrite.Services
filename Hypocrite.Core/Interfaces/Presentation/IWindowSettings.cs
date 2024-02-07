using Hypocrite.Core.Utils;

namespace Hypocrite.Core.Interfaces.Presentation
{
    public interface IWindowSettings
    {
        string LogoImage { get; set; }
        string ProductName { get; set; }
        Visibility MinimizeButtonVisibility { get; set; }
        Visibility MaxResButtonsVisibility { get; set; }
        Visibility WindowProgressVisibility { get; set; }
        bool SmoothAppear { get; set; }
    }
}
