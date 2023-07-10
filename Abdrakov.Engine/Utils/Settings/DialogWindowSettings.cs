using Abdrakov.Engine.Interfaces.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Abdrakov.Engine.Utils.Settings
{
    internal class DialogWindowSettings : IWindowSettings
    {
        public SolidColorBrush WindowHeaderBrush { get; set; } = new SolidColorBrush(Color.FromRgb(63, 68, 79));
        public SolidColorBrush WindowStateBrush { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public string LogoImage { get; set; }
        public string ProductName { get; set; }
        public Visibility MinimizeButtonVisibility { get; set; } = Visibility.Collapsed;
        public Visibility MaxResButtonsVisibility { get; set; } = Visibility.Collapsed;
        public Visibility WindowProgressVisibility { get; set; } = Visibility.Collapsed;
    }
}
