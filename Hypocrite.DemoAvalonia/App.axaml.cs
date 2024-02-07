using Hypocrite.Avalonia.Mvvm;
using Hypocrite.Avalonia.Views;
using Abdrakov.DemoAvalonia.Resources.Themes;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Utils.Settings;
using Hypocrite.Avalonia.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Collections.Generic;
using System.Reflection;
using Abdrakov.DemoAvalonia.Views;
using Abdrakov.DemoAvalonia.Modules;
using Hypocrite.Avalonia.Localization;
using System.Threading;
using Hypocrite.Core.Localization;
using System.Collections.ObjectModel;
using Abdrakov.DemoAvalonia.Views.DialogViews;

namespace Abdrakov.DemoAvalonia
{
    public partial class App : AbdrakovApplication
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
                ProductName = "Abdrakov.DemoAvalonia",
                LogoImage = "avares://Abdrakov.DemoAvalonia/Resources/AbdrakovSolutions.png",
                SmoothAppear = true,
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();

            containerRegistry.RegisterInstance<IThemeSwitcherService<Themes>>(new ThemeSwitcherService<Themes>(Themes.Dark)
            {
                NameOfDictionary = "ThemeHolder",
                ThemeSources = new Dictionary<Themes, string>()
                {
                    { Themes.Dark, "avares://Abdrakov.DemoAvalonia/Resources/Themes/DarkTheme.axaml" },
                    { Themes.Light, "avares://Abdrakov.DemoAvalonia/Resources/Themes/LightTheme.axaml" },
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