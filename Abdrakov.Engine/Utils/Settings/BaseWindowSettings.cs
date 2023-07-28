using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Engine.Utils.Settings
{
    public class BaseWindowSettings : IWindowSettings
    {
        public string LogoImage { get; set; }
        public string ProductName { get; set; }
        public Visibility MinimizeButtonVisibility { get; set; } = Visibility.Visible;
        public Visibility MaxResButtonsVisibility { get; set; } = Visibility.Visible;
        public Visibility WindowProgressVisibility { get; set; } = Visibility.Visible;
        public bool SmoothAppear { get; set; } = true;
    }
}
