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
    public class AbdrakovBundledTheme : ResourceDictionary, IAbdrakovThemeDictionary
    {
        private bool _isDarkMode = true;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                }
            }
        }

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

        public AbdrakovBundledTheme SetTheme()
        {
            if (PrimaryColor is Color primaryColor &&
                SecondaryColor is Color secondaryColor && 
                ScrollBackground is Color scrollBack &&
                ScrollForeground is Color scrollFore)
            {
                ITheme theme = Theme.Create(IsDarkMode, primaryColor, secondaryColor, scrollBack, scrollFore);

                ApplyTheme(theme);
            }
            return this;
        }

        protected virtual void ApplyTheme(ITheme theme) =>
            this.SetTheme(theme);
    }
}
