using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Styles
{
    public class ThemeChangedEventArgs : EventArgs
    {
        public ThemeChangedEventArgs(ResourceDictionary resourceDictionary, ITheme oldTheme, ITheme newTheme, bool isDark)
        {
            ResourceDictionary = resourceDictionary;
            OldTheme = oldTheme;
            NewTheme = newTheme;
            IsDark = isDark;
        }

        public ResourceDictionary ResourceDictionary { get; }
        public ITheme NewTheme { get; }
        public ITheme OldTheme { get; }
        public bool IsDark { get; }
    }
}
