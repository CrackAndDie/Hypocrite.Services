using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Styles
{
    public class AbdrakovBundledTheme : ResourceDictionary, IAbdrakovThemeDictionary
    {
        private bool _isDarkMode = true;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                }
            }
        }

        private InsideBundledTheme _darkTheme;
        public InsideBundledTheme DarkTheme
        {
            get => _darkTheme;
            set
            {
                if (_darkTheme != value)
                {
                    _darkTheme = value;
                }
            }
        }

        private InsideBundledTheme _lightTheme;
        public InsideBundledTheme LightTheme
        {
            get => _lightTheme;
            set
            {
                if (_lightTheme != value)
                {
                    _lightTheme = value;
                }
            }
        }

        private IDictionary<string, ColorPair> _extendedColors;
        public IDictionary<string, ColorPair> ExtendedColors
        {
            get => _extendedColors;
            set
            {
                if (_extendedColors != value)
                {
                    _extendedColors = value;
                }
            }
        }

        public AbdrakovBundledTheme SetTheme()
        {
            ITheme darkTheme = DarkTheme != null ? Theme.Create(DarkTheme, ExtendedColors, true) : null;
            ITheme lightTheme = LightTheme != null ? Theme.Create(LightTheme, ExtendedColors, false) : null;

            ApplyTheme(IsDarkMode, darkTheme, lightTheme);
            return this;
        }

        protected virtual void ApplyTheme(bool isDark, ITheme darkTheme, ITheme lightTheme) =>
            this.SetTheme(isDark, darkTheme, lightTheme);
    }
}
