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

        private Color _scrollForeground = Color.FromRgb(136, 136, 136);
        public Color ScrollForeground
        {
            get => _scrollForeground;
            set
            {
                if (_scrollForeground != value)
                {
                    _scrollForeground = value;
                }
            }
        }

        private Color _scrollBackground = Color.FromRgb(63, 68, 79);
        public Color ScrollBackground
        {
            get => _scrollBackground;
            set
            {
                if (_scrollBackground != value)
                {
                    _scrollBackground = value;
                }
            }
        }

    }
}
