using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Engine.Interfaces.Presentation
{
    public interface IWindowSettings
    {
        SolidColorBrush WindowHeaderBrush { get; set; }
        SolidColorBrush WindowStateBrush { get; set; }
        string LogoImage { get; set; }
        string ProductName { get; set; }
        Visibility MinimizeButtonVisibility { get; set; }
        Visibility MaxResButtonsVisibility { get; set; }
        Visibility WindowProgressVisibility { get; set; }
    }
}
