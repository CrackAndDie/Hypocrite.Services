using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles.Extensions
{
    public static class ThemeExtensions
    {
        public static void SetPrimaryColor(this ITheme theme, Color primaryColor)
        {
            if (theme is null) throw new ArgumentNullException(nameof(theme));

            theme.PrimaryVeryLight = primaryColor.Lighten(2);
            theme.PrimaryLight = primaryColor.Lighten();
            theme.PrimaryMid = primaryColor;
            theme.PrimaryDark = primaryColor.Darken();
            theme.PrimaryVeryDark = primaryColor.Darken(2);
        }

        public static void SetSecondaryColor(this ITheme theme, Color secondaryColor)
        {
            if (theme is null) throw new ArgumentNullException(nameof(theme));

            theme.SecondaryLight = secondaryColor.Lighten();
            theme.SecondaryMid = secondaryColor;
            theme.SecondaryDark = secondaryColor.Darken();
        }

        public static void SetScrollColors(this ITheme theme, Color fore, Color back)
        {
            if (theme is null) throw new ArgumentNullException(nameof(theme));

            theme.ScrollBackground = back;
            theme.ScrollForeground = fore;
        }

        public static void SetOtherColors(this ITheme theme, Color fore)
        {
            if (theme is null) throw new ArgumentNullException(nameof(theme));

            theme.TextForeground = fore;
        }

        public static void SetExtendedColors(this ITheme theme, IDictionary<string, Color> extendedColors)
        {
            theme.ExtendedColors = new Dictionary<string, Color>();
            foreach (var color in extendedColors)
            {
                theme.ExtendedColors[color.Key + "LightBrush"] = color.Value.Lighten();
                theme.ExtendedColors[color.Key + "MidBrush"] = color.Value;
                theme.ExtendedColors[color.Key + "DarkBrush"] = color.Value.Darken();
            }
        }

        public static ITheme GetReversedTheme(this ITheme theme)
        {
            var primary = theme.PrimaryMid.Reverse();
            var secondary = theme.SecondaryMid.Reverse();
            return new Theme()
            {
                PrimaryVeryDark = primary.Darken(2),
                PrimaryDark = primary.Darken(),
                PrimaryMid = primary,
                PrimaryLight = primary.Lighten(),
                PrimaryVeryLight = primary.Lighten(2),

                SecondaryDark = secondary.Darken(),
                SecondaryMid = secondary,
                SecondaryLight = secondary.Lighten(),

                ScrollBackground = theme.ScrollBackground.Reverse(),
                ScrollForeground = theme.ScrollForeground.Reverse(),

                TextForeground = theme.TextForeground.Reverse(),
            };
        }
    }
}
