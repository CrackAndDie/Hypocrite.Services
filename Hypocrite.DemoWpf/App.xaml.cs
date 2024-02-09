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
using System.ComponentModel;
using Hypocrite.DemoWpf.Views;
using Hypocrite.Core.Utils.Settings;
using System.Windows.Media;
using Prism.Modularity;
using Hypocrite.DemoWpf.Modules;
using Hypocrite.Core.Localization;
using System.Collections.ObjectModel;
using Hypocrite.Core.Localization.Extensions;
using System.Threading;
using Hypocrite.DemoWpf.Resources.Themes;
using log4net.Repository.Hierarchy;
using Hypocrite.Mvvm;
using Hypocrite.Localization;
using Hypocrite.Services;
using Hypocrite.DemoWpf.Views.DialogViews;

namespace Hypocrite.DemoWpf
{
    public partial class App : ApplicationBase
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
                ProductName = "Hypocrite.DemoWpf",
                LogoImage = "pack://application:,,,/Hypocrite.DemoWpf;component/Resources/AbdrakovSolutions.png",
                SmoothAppear = true,
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterInstance<IThemeSwitcherService<Themes>>(new ThemeSwitcherService<Themes>()
            {
                NameOfDictionary = "ThemeHolder",
                ThemeSources = new Dictionary<Themes, string>()
                {
                    { Themes.Dark, "/Hypocrite.DemoWpf;component/Resources/Themes/DarkTheme.xaml" },
                    { Themes.Light, "/Hypocrite.DemoWpf;component/Resources/Themes/LightTheme.xaml" },
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
