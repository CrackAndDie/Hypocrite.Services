using Hypocrite.Mvvm;
using Hypocrite.DemoAvalonia.Resources.Themes;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Utils.Settings;
using Hypocrite.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Collections.Generic;
using System.Reflection;
using Hypocrite.DemoAvalonia.Views;
using Hypocrite.DemoAvalonia.Modules;
using Hypocrite.Localization;
using System.Threading;
using Hypocrite.Core.Localization;
using System.Collections.ObjectModel;
using Hypocrite.DemoAvalonia.Views.DialogViews;

namespace Hypocrite.DemoAvalonia
{
    public partial class App : ApplicationBase
    {
        public override void Initialize()
        {
            Thread.CurrentThread.Name = "MainThread";
            LocalizationManager.InitializeExternal(Assembly.GetExecutingAssembly(), new ObservableCollection<Language>()
            {
                new Language() { Name = "EN" },
                new Language() { Name = "RU" },
            });

            AvaloniaXamlLoader.Load(this);
            base.Initialize();              // <-- Required
        }

        protected override AvaloniaObject CreateShell()
        {
            var viewModelService = Container.Resolve<IViewModelResolverService>();
            viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

            return base.CreateShell();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            base.RegisterDefaults(containerRegistry);

            containerRegistry.RegisterSingleton<IPreviewWindow, PreviewWindowView>();

            containerRegistry.RegisterInstance(new BaseWindowSettings()
            {
                ProductName = "Hypocrite.DemoAvalonia",
                LogoImage = "avares://Hypocrite.DemoAvalonia/Resources/AbdrakovSolutions.png",
                SmoothAppear = true,
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterInstance<IThemeSwitcherService<ThemeType>>(new ThemeSwitcherService<ThemeType>(ThemeType.Dark)
            {
                NameOfDictionary = "ThemeHolder",
                ThemeSources = new Dictionary<ThemeType, string>()
                {
                    { ThemeType.Dark, "avares://Hypocrite.DemoAvalonia/Resources/Themes/DarkTheme.axaml" },
                    { ThemeType.Light, "avares://Hypocrite.DemoAvalonia/Resources/Themes/LightTheme.axaml" },
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