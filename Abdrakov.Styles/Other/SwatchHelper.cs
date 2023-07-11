using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other.Swatches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles.Other
{
    public static class SwatchHelper
    {
        public static IEnumerable<ISwatch> Swatches { get; } = new ISwatch[]
        {
            new TestSwatch(),
        };

        public static IDictionary<AbdrakovColor, Color> Lookup { get; } = Swatches.SelectMany(o => o.Lookup).ToDictionary(o => o.Key, o => o.Value);
    }
}
