using Abdrakov.CommonAvalonia.MVVM;
using Abdrakov.CommonAvalonia.Views;
using Abdrakov.DemoAvalonia.Resources.Themes;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Utils.Settings;
using Abdrakov.CommonAvalonia.Services;
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

namespace Abdrakov.DemoAvalonia
{
    public partial class App : AbdrakovApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();              // <-- Required
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

            containerRegistry.RegisterInstance<IThemeSwitcherService<Themes>>(new ThemeSwitcherService<Themes>()
            {
                NameOfDictionary = "ThemeHolder",
                ThemeSources = new Dictionary<Themes, string>()
                {
                    { Themes.Dark, "avares://Abdrakov.DemoAvalonia/Resources/Themes/DarkTheme.axaml" },
                    { Themes.Light, "avares://Abdrakov.DemoAvalonia/Resources/Themes/LightTheme.axaml" },
                },
            });
        }

        protected override AvaloniaObject CreateShell()
        {
            var viewModelService = Container.Resolve<IViewModelResolverService>();
            viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

            return base.CreateShell();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<HeaderModule>();
        }

        /// <summary>Called after <seealso cref="Initialize"/>.</summary>
        protected override void OnInitialized()
        {
            // Register initial Views to Region.
            var regionManager = Container.Resolve<IRegionManager>();
        }
    }
}