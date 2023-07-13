using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Styles.Services
{
    public class AbdrakovThemeService : IAbdrakovThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public bool IsDark => _paletteHelper.GetThemeManager().IsDark;

        public void ApplyBase(bool isDark)
        {
            _paletteHelper.GetThemeManager().ChangeThemeBase(isDark);
        }
    }
}
