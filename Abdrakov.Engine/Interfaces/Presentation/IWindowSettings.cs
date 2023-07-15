using Abdrakov.Engine.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Engine.Interfaces.Presentation
{
    public interface IWindowSettings
    {
        string LogoImage { get; set; }
        string ProductName { get; set; }
        Visibility MinimizeButtonVisibility { get; set; }
        Visibility MaxResButtonsVisibility { get; set; }
        Visibility WindowProgressVisibility { get; set; }
        Visibility ThemeToggleVisibility { get; set; }
        ObservableCollection<Language> AllowedLanguages { get; set; }
        bool AllowTransparency { get; set; }
    }
}
