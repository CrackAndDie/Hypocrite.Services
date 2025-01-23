using Hypocrite.Core.Interfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hypocrite.Events;
using Hypocrite.Container;

namespace Hypocrite.Services
{
    public class ThemeSwitcherService<T> : IThemeSwitcherService<T>
    {
        public string NameOfDictionary { get; set; }
        public IDictionary<T, string> ThemeSources { get; set; }
        public IDictionary<string, Dictionary<string, Dictionary<string, object>>> AdditionalResources { get; set; }

        private T _currentTheme;
        public T CurrentTheme => _currentTheme;

        private readonly Lazy<ResourceDictionary> mainDictionary;
        
        [Injection]
        private IEventAggregator EventAggregator { get; set; }

        public ThemeSwitcherService(T startupTheme)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            mainDictionary = new Lazy<ResourceDictionary>(LoadMainDictionary);
            _currentTheme = startupTheme;
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

            PublishEvent(dic.MergedDictionaries[0], oldTheme, theme);

            _currentTheme = theme;

            return true;
        }

        public bool ChangeAdditionalResource(string category, string type)
        {
            if (!AdditionalResources.ContainsKey(category) || !AdditionalResources[category].ContainsKey(type))
            {
                return false;
            }

            var requestedValues = AdditionalResources[category][type];
            var dic = mainDictionary.Value;
            foreach (var k in requestedValues.Keys)
            {
                dic[k] = requestedValues[k];
            }

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
            // Theme holder should be always the first one in avalonia, cause there is no way of getting ResourceDicitonary name or path
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
