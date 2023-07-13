using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Styles.Other
{
    public class PaletteHelper
    {
        public virtual ITheme GetTheme()
        {
            if (Application.Current is null)
                throw new InvalidOperationException($"Cannot get theme outside of a WPF application. Use {nameof(ResourceDictionaryExtensions)}.{nameof(ResourceDictionaryExtensions.GetTheme)} on the appropriate resource dictionary instead.");
            return GetResourceDictionary().GetTheme();
        }

        public virtual IThemeManager GetThemeManager()
        {
            if (Application.Current is null)
                throw new InvalidOperationException($"Cannot get ThemeManager outside of a WPF application. Use {nameof(ResourceDictionaryExtensions)}.{nameof(ResourceDictionaryExtensions.GetThemeManager)} on the appropriate resource dictionary instead.");
            return GetResourceDictionary().GetThemeManager();
        }

        private static ResourceDictionary GetResourceDictionary()
            => Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x is IAbdrakovThemeDictionary) ??
                Application.Current.Resources;
    }
}
