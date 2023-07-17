using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Abdrakov.Styles
{
    public class InsideBundledTheme
    {
        private Color _primaryColor;
        public Color PrimaryColor
        {
            get => _primaryColor;
            set
            {
                if (_primaryColor != value)
                {
                    _primaryColor = value;
                }
            }
        }

        private Color _secondaryColor;
        public Color SecondaryColor
        {
            get => _secondaryColor;
            set
            {
                if (_secondaryColor != value)
                {
                    _secondaryColor = value;
                }
            }
        }

        private Color _textForegorundColor = Colors.AliceBlue;
        public Color TextForegorundColor
        {
            get => _textForegorundColor;
            set
            {
                if (_textForegorundColor != value)
                {
                    _textForegorundColor = value;
                }
            }
        }
    }
}
