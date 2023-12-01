﻿using Abdrakov.Engine.Interfaces.Presentation;

namespace Abdrakov.Engine.Utils.Settings
{
    public class BaseWindowSettings : IWindowSettings
    {
        public string LogoImage { get; set; }
        public string ProductName { get; set; }
        public Visibility MinimizeButtonVisibility { get; set; } = Visibility.Visible;
        public Visibility MaxResButtonsVisibility { get; set; } = Visibility.Visible;
        public Visibility WindowProgressVisibility { get; set; } = Visibility.Visible;
        public bool SmoothAppear { get; set; } = true;
    }
}
