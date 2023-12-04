using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.CommonAvalonia.Styles.Events
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
