using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
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
        public bool IsDarkMode { get; set; }

        public Color PrimaryLight { get; set; }
        public Color PrimaryMid { get; set; }
        public Color PrimaryDark { get; set; }

        public Color SecondaryLight { get; set; }
        public Color SecondaryMid { get; set; }
        public Color SecondaryDark { get; set; }

        public Color ScrollBackground { get; set; }
        public Color ScrollForeground { get; set; }

        public static Theme Create(bool isDark, Color primary, Color secondary, Color scrollBack, Color scrollFore)
        {
            var theme = new Theme();

            theme.IsDarkMode = isDark;
            theme.SetPrimaryColor(primary);
            theme.SetSecondaryColor(secondary);
            theme.SetScrollColors(scrollFore, scrollBack);

            return theme;
        }
    }
}
