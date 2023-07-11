using Abdrakov.Engine.Interfaces.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Engine.Utils.Settings
{
    public class BaseWindowSettings : IWindowSettings
    {
        public SolidColorBrush WindowHeaderBrush { get; set; } = new SolidColorBrush(Color.FromRgb(63, 68, 79));
        public SolidColorBrush WindowStateBrush { get; set; }
        public string LogoImage { get; set; }
        public string ProductName { get; set; }
        public Visibility MinimizeButtonVisibility { get; set; } = Visibility.Visible;
        public Visibility MaxResButtonsVisibility { get; set; } = Visibility.Visible;
        public Visibility WindowProgressVisibility { get; set; } = Visibility.Visible;
        public bool AllowTransparency { get; set; } = true;
    }
}
