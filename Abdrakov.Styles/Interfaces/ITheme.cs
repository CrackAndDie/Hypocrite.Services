using Abdrakov.Styles.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles.Interfaces
{
    public interface ITheme
    {
        Color PrimaryVeryLight { get; set; }
        Color PrimaryLight { get; set; }
        Color PrimaryMid { get; set; }
        Color PrimaryDark { get; set; }
        Color PrimaryVeryDark { get; set; }

        Color SecondaryLight { get; set; }
        Color SecondaryMid { get; set; }
        Color SecondaryDark { get; set; }

        Color ScrollBackground { get; set; }
        Color ScrollForeground { get; set; }

        Color TextForeground { get; set; }

        IDictionary<string, Color> ExtendedColors { get; set; }
    }
}
