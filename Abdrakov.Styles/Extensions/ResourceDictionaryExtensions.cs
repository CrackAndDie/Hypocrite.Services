using Abdrakov.Engine.MVVM;
using Abdrakov.Styles.Interfaces;
using Prism.Events;
using Prism.Unity;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Abdrakov.Styles.Other.Event;
using System.Web.UI;

namespace Abdrakov.Styles.Extensions
{
    public static class ResourceDictionaryExtensions
    {
        private const string CurrentThemeKey = nameof(Abdrakov) + "." + nameof(CurrentThemeKey);
        private const string ThemeManagerKey = nameof(Abdrakov) + "." + nameof(ThemeManagerKey);

        public static void SetTheme(this ResourceDictionary resourceDictionary, bool isDark, ITheme darkTheme, ITheme lightTheme)
        {
            if (resourceDictionary is null) throw new ArgumentNullException(nameof(resourceDictionary));

            var theme = isDark ? darkTheme : lightTheme;

            if (theme.ExtendedColors != null)
            {
                foreach (var color in theme.ExtendedColors)
                {
                    SetSolidColorBrush(resourceDictionary, color.Key, color.Value);
                }
            }

            if (!(resourceDictionary.GetThemeManager() is ThemeManager))
            {
                resourceDictionary[ThemeManagerKey] = new ThemeManager(resourceDictionary, isDark, darkTheme, lightTheme);
            }
            ITheme oldTheme = resourceDictionary.GetTheme();
            resourceDictionary[CurrentThemeKey] = theme;

            var cont = (Application.Current as AbdrakovApplication).Container;
            if (cont.IsRegistered<IEventAggregator>())
            {
                cont.Resolve<IEventAggregator>().GetEvent<ThemeChangedEvent>()
                .Publish(new ThemeChangedEventArgs(resourceDictionary, oldTheme, theme, isDark));
            }
        }

        public static ITheme GetTheme(this ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary is null) throw new ArgumentNullException(nameof(resourceDictionary));
            if (resourceDictionary[CurrentThemeKey] is ITheme theme)
            {
                return theme;
            }
            return null;
        }

        public static IThemeManager GetThemeManager(this ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary is null) throw new ArgumentNullException(nameof(resourceDictionary));
            return resourceDictionary[ThemeManagerKey] as IThemeManager;
        }

        internal static void SetSolidColorBrush(this ResourceDictionary sourceDictionary, string name, Color value)
        {
            if (sourceDictionary is null) throw new ArgumentNullException(nameof(sourceDictionary));
            if (name is null) throw new ArgumentNullException(nameof(name));

            sourceDictionary[name + "Color"] = value;

            if (sourceDictionary[name] is SolidColorBrush brush)
            {
                if (brush.Color == value) return;

                if (!brush.IsFrozen)
                {
                    var animation = new ColorAnimation
                    {
                        From = brush.Color,
                        To = value,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                    return;
                }
            }

            var newBrush = new SolidColorBrush(value);
            newBrush.Freeze();
            sourceDictionary[name] = newBrush;
        }
    }
}
