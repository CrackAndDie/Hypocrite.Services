using Abdrakov.CommonAvalonia.Styles.Events;
using Abdrakov.Container;
using Abdrakov.Engine.Interfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abdrakov.CommonAvalonia.Services
{
    public class ThemeSwitcherService<T> : IThemeSwitcherService<T>
    {
        public string NameOfDictionary { get; set; }
        public IDictionary<T, string> ThemeSources { get; set; }
        private T _currentTheme;
        public T CurrentTheme => _currentTheme;

        private readonly Lazy<ResourceDictionary> mainDictionary;

        [Injection]
        private IEventAggregator EventAggregator { get; set; }

        public ThemeSwitcherService()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            mainDictionary = new Lazy<ResourceDictionary>(LoadMainDictionary);
        }

        public bool ChangeTheme(T theme)
        {
            if (!ThemeSources.Keys.Contains(theme))
            {
                return false;
            }

            T oldTheme = CurrentTheme;

            var dic = mainDictionary.Value;
            dic.MergedDictionaries.RemoveAt(0);
            Uri uri = new Uri(ThemeSources[theme]);
            dic.MergedDictionaries.Add(new ResourceInclude(uri) { Source = uri });

            PublishEvent(dic.MergedDictionaries[0], oldTheme, CurrentTheme);

            _currentTheme = theme;

            return true;
        }

        public object TryFindResource(object name)
        {
            if (Application.Current.TryFindResource(name, Application.Current.ActualThemeVariant, out object val))
            {
                return val;
            }
            return null;
        }

        public object TryGetResourceFromTheme(object name, T theme)
        {
            if (!ThemeSources.Keys.Contains(theme))
            {
                return false;
            }
            var dic = new ResourceInclude(new Uri(ThemeSources[theme]));
            if (dic.TryGetResource(name, Application.Current.ActualThemeVariant, out object val))
            {
                return val;
            }
            return null;
        }

        private ResourceDictionary LoadMainDictionary()
        {
            // Theme holder should be always the first one
            var resourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
            if (resourceDictionary == null)
            {
                throw new KeyNotFoundException($"Could not load main resource dictionary: {NameOfDictionary}");
            }
            return resourceDictionary as ResourceDictionary;
        }

        private void PublishEvent(IResourceProvider dic, T oldTheme, T newTheme)
        {
            EventAggregator.GetEvent<ThemeChangedEvent<T>>()
                .Publish(new ThemeChangedEventArgs<T>(dic as ResourceDictionary, oldTheme, newTheme));
        }
    }
}
