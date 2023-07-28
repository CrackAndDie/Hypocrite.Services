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
        public IDictionary<string, Color> ExtendedColors { get; set; }

        public static Theme Create(IDictionary<string, ColorPair> extended, bool isDark)
        {
            var theme = new Theme();
            if (extended != null)
            {
                theme.SetExtendedColors(extended.Select(x => new { Key = x.Key, Value = isDark ? x.Value.DarkColor : x.Value.LightColor }).ToDictionary(x => x.Key, x => x.Value));
            }

            return theme;
        }
    }
}
