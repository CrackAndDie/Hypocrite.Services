﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Styles.Interfaces
{
    public interface IThemeManager
    {
        event EventHandler<ThemeChangedEventArgs> ThemeChanged;
        void ChangeThemeBase(bool isDark);
    }
}