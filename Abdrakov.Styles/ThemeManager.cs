using Abdrakov.Styles.Extensions;
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
        private bool _isDark;
        private ITheme _darkTheme;
        private ITheme _lightTheme;

        public ThemeManager(ResourceDictionary resourceDictionary, bool isDark, ITheme darkTheme, ITheme lightTheme)
        {
            _ResourceDictionary = resourceDictionary ?? throw new ArgumentNullException(nameof(resourceDictionary));
            _isDark = isDark;

            var theme = resourceDictionary.GetTheme();
            _darkTheme = darkTheme == null ? (_isDark ? theme : theme.GetReversedTheme()) : darkTheme;
            _lightTheme = lightTheme == null ? (_isDark ? theme.GetReversedTheme() : theme) : lightTheme;
        }

        public void ChangeThemeBase(bool isDark)
        {
            if (isDark != _isDark)
            {
                _isDark = isDark;
                _ResourceDictionary.SetTheme(_isDark, _darkTheme, _lightTheme);
            }
        }

        public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        public void OnThemeChange(ITheme oldTheme, ITheme newTheme)
            => ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(_ResourceDictionary, oldTheme, newTheme));
    }
}
