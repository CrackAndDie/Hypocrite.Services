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

            theme.PrimaryLight = primaryColor.Lighten();
            theme.PrimaryMid = primaryColor;
            theme.PrimaryDark = primaryColor.Darken();
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

        public static ITheme GetReversedTheme(this ITheme theme)
        {
            var primary = theme.PrimaryMid.Reverse();
            var secondary = theme.SecondaryMid.Reverse();
            var scrollBack = theme.ScrollBackground.Reverse();
            var scrollFore = theme.ScrollForeground.Reverse();
            return new Theme()
            {
                PrimaryDark = primary.Darken(),
                PrimaryMid = primary,
                PrimaryLight = primary.Lighten(),

                SecondaryDark = secondary.Darken(),
                SecondaryMid = secondary,
                SecondaryLight = secondary.Lighten(),

                ScrollBackground = scrollBack,
                ScrollForeground = scrollFore,
            };
        }
    }
}
