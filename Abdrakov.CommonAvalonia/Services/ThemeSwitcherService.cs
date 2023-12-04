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
        public T CurrentTheme => GetCurrentTheme();

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
            dic.MergedDictionaries.Add(new ResourceInclude(new Uri(ThemeSources[theme])));

            PublishEvent(dic.MergedDictionaries[0], oldTheme, CurrentTheme);

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
            var resourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => (x as ResourceInclude).Source.OriginalString.Contains(NameOfDictionary));
            if (resourceDictionary == null)
            {
                throw new KeyNotFoundException($"Could not load main resource dictionary: {NameOfDictionary}");
            }
            return resourceDictionary as ResourceDictionary;
        }

        private T GetCurrentTheme()
        {
            var dic = mainDictionary.Value;
            var currDic = dic.MergedDictionaries.FirstOrDefault();
            if (currDic == null)
            {
                throw new NotImplementedException("Current theme could not be found");
            }
            return ThemeSources.FirstOrDefault(x => x.Value == (currDic as ResourceInclude).Source.OriginalString).Key;
        }

        private void PublishEvent(IResourceProvider dic, T oldTheme, T newTheme)
        {
            EventAggregator.GetEvent<ThemeChangedEvent<T>>()
                .Publish(new ThemeChangedEventArgs<T>(dic as ResourceDictionary, oldTheme, newTheme));
        }
    }
}
