using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Styles
{
    public class ThemeManager : IThemeManager
    {
        private ResourceDictionary _ResourceDictionary;

        public ThemeManager(ResourceDictionary resourceDictionary)
            => _ResourceDictionary = resourceDictionary ?? throw new ArgumentNullException(nameof(resourceDictionary));

        public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        public void OnThemeChange(ITheme oldTheme, ITheme newTheme)
            => ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(_ResourceDictionary, oldTheme, newTheme));
    }
}
