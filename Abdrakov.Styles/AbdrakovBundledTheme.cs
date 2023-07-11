using Abdrakov.Styles.Extensions;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Styles
{
    internal class AbdrakovBundledTheme : ResourceDictionary
    {
        private PrimaryColor? _primaryColor;
        public PrimaryColor? PrimaryColor
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

        private void SetTheme()
        {
            if (PrimaryColor is PrimaryColor primaryColor)
            {
                ITheme theme = Theme.Create(SwatchHelper.Lookup[(AbdrakovColor)primaryColor]);

                ApplyTheme(theme);
            }
        }

        protected virtual void ApplyTheme(ITheme theme) =>
            this.SetTheme(theme);
    }
}
