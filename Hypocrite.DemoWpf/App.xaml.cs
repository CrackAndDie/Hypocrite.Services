using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Mvvm;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Hypocrite.Wpf.Views;
using System.ComponentModel;
using Abdrakov.Demo.Views;
using Hypocrite.Core.Utils.Settings;
using System.Windows.Media;
using Prism.Modularity;
using Abdrakov.Demo.Modules;
using Hypocrite.Core.Localization;
using System.Collections.ObjectModel;
using Hypocrite.Core.Localization.Extensions;
using System.Threading;
using Abdrakov.Demo.Resources.Themes;
using log4net.Repository.Hierarchy;
using Hypocrite.Wpf.Mvvm;
using Hypocrite.Wpf.Localization;
using Hypocrite.Wpf.Services;
using Abdrakov.Demo.Views.DialogViews;

namespace Abdrakov.Demo
{
    public partial class App : AbdrakovApplication
    {
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
            base.RegisterDefaults(containerRegistry);

            containerRegistry.RegisterSingleton<IPreviewWindow, PreviewWindowView>();

            containerRegistry.RegisterInstance(new BaseWindowSettings()
            {
                ProductName = "Abdrakov.Demo",
                LogoImage = "pack://application:,,,/Abdrakov.Demo;component/Resources/AbdrakovSolutions.png",
                SmoothAppear = true,
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterInstance<IThemeSwitcherService<Themes>>(new ThemeSwitcherService<Themes>()
            {
                NameOfDictionary = "ThemeHolder",
                ThemeSources = new Dictionary<Themes, string>()
                {
                    { Themes.Dark, "/Abdrakov.Demo;component/Resources/Themes/DarkTheme.xaml" },
                    { Themes.Light, "/Abdrakov.Demo;component/Resources/Themes/LightTheme.xaml" },
                },
            });

            containerRegistry.RegisterDialog<MessageDialogView>();
            containerRegistry.RegisterDialogWindow<DialogWindowView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<HeaderModule>();
        }
    }
}
