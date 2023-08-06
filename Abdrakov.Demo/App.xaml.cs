using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM;
using Abdrakov.Styles;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Abdrakov.CommonWPF.Views;
using System.ComponentModel;
using Abdrakov.Demo.Interfaces;
using Abdrakov.Demo.Views;
using Abdrakov.Engine.Utils.Settings;
using System.Windows.Media;
using Prism.Modularity;
using Abdrakov.Demo.Modules;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Services;
using Abdrakov.Styles.Other;
using Abdrakov.Engine.Localization;
using System.Collections.ObjectModel;
using Abdrakov.Engine.Localization.Extensions;
using System.Threading;

namespace Abdrakov.Demo
{
    public partial class App : AbdrakovApplication
    {
        public App() : base()
        {
            ConfigureApplicationVisual();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.Name = "MainThread";
            LocalizationManager.InitializeExternal(Assembly.GetExecutingAssembly(), new ObservableCollection<Language>()
            {
                new Language() { Name = "EN" },
                new Language() { Name = "RU" },
            });
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            var viewModelService = Container.Resolve<IViewModelResolverService>();
            viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

            return base.CreateShell();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            containerRegistry.RegisterSingleton<IPreviewWindow, PreviewWindowView>();

            containerRegistry.RegisterInstance(new BaseWindowSettings()
            {
                ProductName = "Tests",
                LogoImage = "pack://application:,,,/Abdrakov.Tests;component/Resources/AbdrakovSolutions.png",
                SmoothAppear = true,
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterSingleton<IAbdrakovThemeService, AbdrakovThemeService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
        }

        private void ConfigureApplicationVisual()
        {
            Resources.MergedDictionaries.Add(new AbdrakovBundledTheme()
            {
                IsDarkMode = true,
                ExtendedColors = new Dictionary<string, ColorPair>()
                {
                    { "TextForeground", new ColorPair(Colors.AliceBlue, Colors.Black) },
                    { "WindowStatus", new ColorPair(Colors.Cyan, Colors.Cyan) },
                    { "Window", new ColorPair(Color.FromRgb(64, 64, 64), Color.FromRgb(254, 254, 254)) },

                    { "Test", new ColorPair(Colors.Red, Colors.Purple) },

                    { "ButtonBorder", new ColorPair(Colors.Cyan, Colors.Cyan) },

                    { "ComboBoxBorder", new ColorPair(Colors.Cyan, Colors.Cyan) },
                    { "ComboBoxBackground", new ColorPair(Color.FromRgb(64, 64, 64), Color.FromRgb(254, 254, 254)) },
                    { "ComboBoxHoverBackground", new ColorPair(Color.FromRgb(84, 84, 84), Color.FromRgb(234, 234, 234)) },

                    { "ScrollBackground", new ColorPair(Color.FromRgb(63, 68, 79), Colors.AliceBlue) },
                    { "ScrollForeground", new ColorPair(Color.FromRgb(136, 136, 136), Colors.LightGray) },
                }
            }.SetTheme());
        }
    }
}
