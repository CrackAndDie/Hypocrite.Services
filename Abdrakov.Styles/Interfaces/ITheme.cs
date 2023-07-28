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
        IDictionary<string, Color> ExtendedColors { get; set; }
    }
}
