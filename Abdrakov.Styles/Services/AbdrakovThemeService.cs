using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other;
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

        public void ApplyBase(bool isDark)
        {
            _paletteHelper.GetThemeManager().ChangeThemeBase(isDark);
        }
    }
}
