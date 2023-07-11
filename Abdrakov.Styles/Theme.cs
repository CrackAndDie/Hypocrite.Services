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
        public Color PrimaryLight { get; set; }
        public Color PrimaryMid { get; set; }
        public Color PrimaryDark { get; set; }

        public Color ScrollBackground { get; set; }
        public Color ScrollForeground { get; set; }

        public static Theme Create(Color primary)
        {
            var theme = new Theme();

            theme.SetPrimaryColor(primary);

            return theme;
        }
    }
}
