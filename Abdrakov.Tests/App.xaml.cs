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
using Abdrakov.Tests.Interfaces;
using Abdrakov.Tests.Views;
using Abdrakov.Engine.Utils.Settings;
using System.Windows.Media;
using Prism.Modularity;
using Abdrakov.Tests.Modules;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Services;
using Abdrakov.Styles.Other;
using Abdrakov.Engine.Localization;
using System.Collections.ObjectModel;
using Abdrakov.Engine.Localization.Extensions;

namespace Abdrakov.Tests
{
    public partial class App : AbdrakovApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalizationManager.InitializeExternal(Assembly.GetExecutingAssembly());
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
                AllowedLanguages = new ObservableCollection<Language>() { new Language(0, "RU"), new Language(1, "EN"), },
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterSingleton<IAbdrakovThemeService, AbdrakovThemeService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // should be called right here
            ConfigureApplicationVisual();

            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
        }

        private void ConfigureApplicationVisual()
        {
            Resources.MergedDictionaries.Add(new AbdrakovBundledTheme()
            {
                IsDarkMode = true,
                DarkTheme = new InsideBundledTheme()
                {
                    PrimaryColor = Color.FromRgb(64, 64, 64),
                    SecondaryColor = Colors.HotPink,
                },
                LightTheme = new InsideBundledTheme()
                {
                    PrimaryColor = Color.FromRgb(254, 254, 254),
                    SecondaryColor = Colors.HotPink,
                    ScrollBackground = Colors.AliceBlue,
                    ScrollForeground = Colors.LightGray,
                    TextForegorundColor = Colors.Black,
                },
                ExtendedColors = new Dictionary<string, ColorPair>()
                {
                    { "Test", new ColorPair(Colors.Red, Colors.Purple) },
                }
            }.SetTheme());
        }
    }
}
