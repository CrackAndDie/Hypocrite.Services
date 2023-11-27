using System.Windows;

namespace Abdrakov.CommonWPF.Styles.Events
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
