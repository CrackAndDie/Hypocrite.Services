using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Abdrakov.Styles
{
    internal class AbdrakovBundledTheme : ResourceDictionary
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
                    SetTheme();
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
                    SetTheme();
                }
            }
        }

        private Color _scrollForeground;
        public Color ScrollForeground
        {
            get => _scrollForeground;
            set
            {
                if (_scrollForeground != value)
                {
                    _scrollForeground = value;
                    SetTheme();
                }
            }
        }

        private Color _scrollBackground;
        public Color ScrollBackground
        {
            get => _scrollBackground;
            set
            {
                if (_scrollBackground != value)
                {
                    _scrollBackground = value;
                    SetTheme();
                }
            }
        }

        private void SetTheme()
        {
            if (PrimaryColor is Color primaryColor &&
                SecondaryColor is Color secondaryColor && 
                ScrollBackground is Color scrollBack &&
                ScrollForeground is Color scrollFore)
            {
                ITheme theme = Theme.Create(primaryColor, secondaryColor, scrollBack, scrollFore);

                ApplyTheme(theme);
            }
        }

        protected virtual void ApplyTheme(ITheme theme) =>
            this.SetTheme(theme);
    }
}
