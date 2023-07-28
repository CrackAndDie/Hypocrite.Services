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
            ITheme darkTheme = Theme.Create(ExtendedColors, true);
            ITheme lightTheme = Theme.Create(ExtendedColors, false);

            ApplyTheme(IsDarkMode, darkTheme, lightTheme);
            return this;
        }

        protected virtual void ApplyTheme(bool isDark, ITheme darkTheme, ITheme lightTheme) =>
            this.SetTheme(isDark, darkTheme, lightTheme);
    }
}
