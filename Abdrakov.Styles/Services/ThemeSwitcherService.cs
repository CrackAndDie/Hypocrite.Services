using Abdrakov.Engine.MVVM;
using Abdrakov.Styles.Events;
using Abdrakov.Styles.Interfaces;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Abdrakov.Styles.Services
{
    public class ThemeSwitcherService<T> : IThemeSwitcherService<T>
    {
        public string NameOfDictionary { get; set; }
        public IDictionary<T, string> ThemeSources { get; set; }
        public T CurrentTheme => GetCurrentTheme();

        private readonly Lazy<ResourceDictionary> mainDictionary;

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
            dic.BeginInit();
            dic.MergedDictionaries.RemoveAt(0);
            dic.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri(ThemeSources[theme], UriKind.RelativeOrAbsolute)
            });
            dic.EndInit();

            PublishEvent(dic.MergedDictionaries[0], oldTheme, CurrentTheme);

            return true;
        }

        public object TryFindResource(object name)
        {
            return Application.Current.TryFindResource(name);
        }

        public object TryGetResourceFromTheme(object name, T theme)
        {
            if (!ThemeSources.Keys.Contains(theme))
            {
                return false;
            }
            var dic = new ResourceDictionary()
            {
                Source = new Uri(ThemeSources[theme], UriKind.RelativeOrAbsolute)
            };
            return dic.Contains(name) ? dic[name] : null;
        }

        private ResourceDictionary LoadMainDictionary()
        {
            var resourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.OriginalString.Contains(NameOfDictionary));
            if (resourceDictionary == null)
            {
                throw new ResourceReferenceKeyNotFoundException("Could not load main resource dictionary", NameOfDictionary);
            }
            return resourceDictionary;
        }

        private T GetCurrentTheme()
        {
            var dic = mainDictionary.Value;
            var currDic = dic.MergedDictionaries.FirstOrDefault();
            if (currDic == null)
            {
                throw new NotImplementedException("Current theme could not be found");
            }
            return ThemeSources.FirstOrDefault(x => x.Value == currDic.Source.OriginalString).Key;
        }

        private void PublishEvent(ResourceDictionary dic, T oldTheme, T newTheme)
        {
            var cont = (Application.Current as AbdrakovApplication).Container;
            if (cont.IsRegistered<IEventAggregator>())
            {
                cont.Resolve<IEventAggregator>().GetEvent<ThemeChangedEvent<T>>()
                .Publish(new ThemeChangedEventArgs<T>(dic, oldTheme, newTheme));
            }
        }
    }
}
