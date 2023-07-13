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
        public Color PrimaryVeryLight { get; set; }
        public Color PrimaryLight { get; set; }
        public Color PrimaryMid { get; set; }
        public Color PrimaryDark { get; set; }
        public Color PrimaryVeryDark { get; set; }

        public Color SecondaryLight { get; set; }
        public Color SecondaryMid { get; set; }
        public Color SecondaryDark { get; set; }

        public Color ScrollBackground { get; set; }
        public Color ScrollForeground { get; set; }

        public Color TextForeground { get; set; }

        public static Theme Create(InsideBundledTheme insideTheme)
        {
            var theme = new Theme();

            theme.SetPrimaryColor(insideTheme.PrimaryColor);
            theme.SetSecondaryColor(insideTheme.SecondaryColor);
            theme.SetScrollColors(insideTheme.ScrollForeground, insideTheme.ScrollBackground);
            theme.SetOtherColors(insideTheme.TextForegorundColor);

            return theme;
        }
    }
}
