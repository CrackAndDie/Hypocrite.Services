using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles
{
    public class Theme : ITheme
    {
        public Color PrimaryLight { get; set; }
        public Color PrimaryMid { get; set; }
        public Color PrimaryDark { get; set; }

        public Color NonPrimary { get; set; }

        public Color SecondaryLight { get; set; }
        public Color SecondaryMid { get; set; }
        public Color SecondaryDark { get; set; }

        public Color TextForeground { get; set; }

        public IDictionary<string, Color> ExtendedColors { get; set; }

        public static Theme Create(InsideBundledTheme insideTheme, IDictionary<string, ColorPair> extended, bool isDark)
        {
            var theme = new Theme();

            theme.SetPrimaryColor(insideTheme.PrimaryColor);
            theme.SetSecondaryColor(insideTheme.SecondaryColor);
            theme.SetOtherColors(insideTheme.TextForegorundColor);
            if (extended != null)
            {
                theme.SetExtendedColors(extended.Select(x => new { Key = x.Key, Value = isDark ? x.Value.DarkColor : x.Value.LightColor }).ToDictionary(x => x.Key, x => x.Value));
            }

            return theme;
        }
    }
}
