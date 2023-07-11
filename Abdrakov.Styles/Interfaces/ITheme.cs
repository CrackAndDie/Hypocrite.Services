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
        Color PrimaryLight { get; set; }
        Color PrimaryMid { get; set; }
        Color PrimaryDark { get; set; }

        Color ScrollBackground { get; set; }
        Color ScrollForeground { get; set; }
    }
}
