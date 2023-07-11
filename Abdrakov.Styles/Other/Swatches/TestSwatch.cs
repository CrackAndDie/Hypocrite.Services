using Abdrakov.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles.Other.Swatches
{
    public class TestSwatch : ISwatch
    {
        public static Color Test0 { get; } = (Color)ColorConverter.ConvertFromString("#FFEBEE");

        public string Name { get; } = "Test";

        public IDictionary<AbdrakovColor, Color> Lookup { get; } = new Dictionary<AbdrakovColor, Color>
        {
            { AbdrakovColor.Test0, Test0 },
        };

        public IEnumerable<Color> Hues => Lookup.Values;
    }
}
