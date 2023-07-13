using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles.Other
{
    public class ColorPair
    {
        public Color DarkColor { get; private set; }
        public Color LightColor { get; private set; }

        public ColorPair(Color dark, Color light) 
        {
            DarkColor = dark;
            LightColor = light;
        }
    }
}
