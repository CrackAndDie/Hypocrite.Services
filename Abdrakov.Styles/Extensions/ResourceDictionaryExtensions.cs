using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Abdrakov.Styles.Extensions
{
    public static class ResourceDictionaryExtensions
    {
        private const string CurrentThemeKey = nameof(Abdrakov) + "." + nameof(CurrentThemeKey);
        private const string ThemeManagerKey = nameof(Abdrakov) + "." + nameof(ThemeManagerKey);

        public static void SetTheme(this ResourceDictionary resourceDictionary, ITheme theme)
        {
            if (resourceDictionary is null) throw new ArgumentNullException(nameof(resourceDictionary));

            SetSolidColorBrush(resourceDictionary, "PrimaryLightBrush", theme.PrimaryLight);
            SetSolidColorBrush(resourceDictionary, "PrimaryMidBrush", theme.PrimaryMid);
            SetSolidColorBrush(resourceDictionary, "PrimaryDarkBrush", theme.PrimaryDark);

            SetSolidColorBrush(resourceDictionary, "SecondaryLightBrush", theme.SecondaryLight);
            SetSolidColorBrush(resourceDictionary, "SecondaryMidBrush", theme.SecondaryMid);
            SetSolidColorBrush(resourceDictionary, "SecondaryDarkBrush", theme.SecondaryDark);

            SetSolidColorBrush(resourceDictionary, "ScrollBackgroundBrush", theme.ScrollBackground);
            SetSolidColorBrush(resourceDictionary, "ScrollForegroundBrush", theme.ScrollForeground);

            if (!(resourceDictionary.GetThemeManager() is ThemeManager themeManager))
            {
                resourceDictionary[ThemeManagerKey] = themeManager = new ThemeManager(resourceDictionary, theme.IsDarkMode);
            }
            ITheme oldTheme = resourceDictionary.GetTheme();
            resourceDictionary[CurrentThemeKey] = theme;

            themeManager.OnThemeChange(oldTheme, theme);
        }

        public static ITheme GetTheme(this ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary is null) throw new ArgumentNullException(nameof(resourceDictionary));
            if (resourceDictionary[CurrentThemeKey] is ITheme theme)
            {
                return theme;
            }

            //Attempt to simply look up the appropriate resources
            return new Theme
            {
                PrimaryLight = GetColor("PrimaryLightBrush"),
                PrimaryMid = GetColor("PrimaryMidBrush"),
                PrimaryDark = GetColor("PrimaryDarkBrush"),

                SecondaryLight = GetColor("SecondaryLightBrush"),
                SecondaryMid = GetColor("SecondaryMidBrush"),
                SecondaryDark = GetColor("SecondaryDarkBrush"),

                ScrollBackground = GetColor("ScrollBackgroundBrush"),
                ScrollForeground = GetColor("ScrollForegroundBrush"),
            };

            Color GetColor(params string[] keys)
            {
                foreach (string key in keys)
                {
                    if (TryGetColor(key, out Color color))
                    {
                        return color;
                    }
                }
                throw new InvalidOperationException($"Could not locate required resource with key(s) '{string.Join(", ", keys)}'");
            }

            bool TryGetColor(string key, out Color color)
            {
                if (resourceDictionary[key] is SolidColorBrush brush)
                {
                    color = brush.Color;
                    return true;
                }
                color = default;
                return false;
            }
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
