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
        public static void SetExtendedColors(this ITheme theme, IDictionary<string, Color> extendedColors)
        {
            theme.ExtendedColors = new Dictionary<string, Color>();
            foreach (var color in extendedColors)
            {
                theme.ExtendedColors[color.Key + "Brush"] = color.Value;
            }
        }
    }
}
