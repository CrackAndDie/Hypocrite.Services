using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Styles.Events
{
    public class ThemeChangedEventArgs<T>
    {
        public ThemeChangedEventArgs(ResourceDictionary resourceDictionary, T oldTheme, T newTheme)
        {
            ResourceDictionary = resourceDictionary;
            OldTheme = oldTheme;
            NewTheme = newTheme;
        }

        public ResourceDictionary ResourceDictionary { get; }
        public T NewTheme { get; }
        public T OldTheme { get; }
    }
}
