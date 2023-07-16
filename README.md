<p align="center">
  <a href="https://robocadsim.readthedocs.io/en/latest/index.html">
    <img src="https://raw.githubusercontent.com/CADindustries/container/main/logos/AbdrakovSolutions.png" alt="Abdrakov.Solutions logo" width="340" height="340">
  </a>
</p>
<h1 align="center">Abdrakov.Solutions</h1>

<h2>About:</h2>  

This is the main repo for all the Abdrakov projects. This project fully supports Prism features and MVVM pattern. 

<h2>Getting started:</h2>  

<h3>First steps:</h3>  

When you created your WPF app you should rewrite your *App.xaml* and *App.xaml.cs* files as follows:
```xaml
<engine:AbdrakovApplication x:Class="Abdrakov.Tests.App"
                            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                            xmlns:local="clr-namespace:Abdrakov.Tests"
                            xmlns:engine="clr-namespace:Abdrakov.Engine.MVVM;assembly=Abdrakov.Engine">
</engine:AbdrakovApplication>
```

```cs
namespace YourNamespace
{
    public partial class App : AbdrakovApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
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
            containerRegistry.RegisterInstance(new BaseWindowSettings()
            {
                ProductName = "Tests",
                LogoImage = "pack://application:,,,/Abdrakov.Tests;component/Resources/AbdrakovSolutions.png",
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            // ...
        }
    }
}
```

As you can see there is a *BaseWindowSettings* registration that gives you quite flexible setting of your Window (in this example we use *MainWindowView* from *Abdrakov.CommonWPF* namespace). Here you can see all the available settings of the Window:
- *LogoImage* - Path to the logo image of your app like ```"pack://application:,,,/Abdrakov.Tests;component/Resources/AbdrakovSolutions.png"```
- *ProductName* - Title text that will be shown on the window header
- *MinimizeButtonVisibility* - How should be Minimize button be shown (default is Visible)
- *MaxResButtonsVisibility* - How should be Maximize and Restore buttons be shown (default is Visible)
- *WindowProgressVisibility* - How should be Progress state be shown (default is Visible)
- *ThemeToggleVisibility* - How should be Theme toggler be shown (default is Visible)
- *AllowedLanguages* - ObservableCollection of Languages available in app (it is empty by default so it won't be shown)
- *AllowTransparency* - I don't remember why there is the setting (default is True)

You can use *Regions.MAIN_REGION* to navigate in the *MainWindowView*:
```cs
internal class MainModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var region = containerProvider.Resolve<IRegionManager>();
        // the view to display on start up
        region.RegisterViewWithRegion(Regions.MAIN_REGION, typeof(MainPageView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<MainPageView>();
    }
}
```

<h3>Preview window:</h3>  

You can create your own preview window using *Abdrakov.Solutions*. Your preview window has to implement *IPreviewWindow* interface. Here is the sample preview window that shows up for 4 seconds:
```cs
public partial class PreviewWindowView : Window, IPreviewWindow
{
    private DispatcherTimer timer;
    public PreviewWindowView()
    {
        InitializeComponent();

        timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(4),
        };
        timer.Tick += (s, a) => { CallPreviewDoneEvent(); };
        timer.Start();
    }

    public void CallPreviewDoneEvent()
    {
        timer.Stop();
        var cont = (Application.Current as AbdrakovApplication).Container;
        cont.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Publish();
        this.Close();
    }
}
```

Your preview window should be also registered like this:
```cs
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    base.RegisterTypes(containerRegistry);

    containerRegistry.RegisterSingleton<IPreviewWindow, PreviewWindowView>();

    // ...
}
```

Registered Window under *IBaseWindow* interface will be shown up after *PreviewDoneEvent* event call.

<h3>Theme registrations:</h3>  

Abdrakov.Solutions supports realtime theme change (Dark/Light) and flexible colors (brushes) registrations. Here is the sample code to register *IAbdrakovThemeService* in your App (if you don't want the theme change service in your project you can skip this registration):
```cs
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    base.RegisterTypes(containerRegistry);
    // ...
    containerRegistry.RegisterSingleton<IAbdrakovThemeService, AbdrakovThemeService>();
}
```

Using the *IAbdrakovThemeService* you can change an App's theme in realtime using method *ApplyBase(isDark)* and get current theme using property *IsDark*.  
Here is the sample code to register themes and colors in your App:
```cs
private void ConfigureApplicationVisual()
{
    Resources.MergedDictionaries.Add(new AbdrakovBundledTheme()
    {
        IsDarkMode = true,  // default theme on app startup
        DarkTheme = new InsideBundledTheme()  // dark theme registration
        {
            PrimaryColor = Color.FromRgb(64, 64, 64),  // primary color of dark theme
            SecondaryColor = Colors.HotPink,  // secondary color of dark theme
        },
        LightTheme = new InsideBundledTheme()  // light theme registration
        {
            PrimaryColor = Color.FromRgb(254, 254, 254),  // primary color of light theme
            SecondaryColor = Colors.HotPink,  // secondary color of light theme
            ScrollBackground = Colors.AliceBlue,  // scrollviewer background of light theme (should be removed in the next versions)
            ScrollForeground = Colors.LightGray,  // scrollviewer foreground of light theme (should be removed in the next versions)
            TextForegorundColor = Colors.Black,  // text foreground of light theme
        },
        ExtendedColors = new Dictionary<string, ColorPair>()  // your external color registrations
        {
            { "Test", new ColorPair(Colors.Red, Colors.Purple) },
        }
    }.SetTheme());
}
```
Every external color registration gives you three dynamic Brushes and Colors (like *TestLightBrush*, *TestMidBrush*, *TestDarkBrush* and *TestLightBrushColor*, *TestMidBrushColor*, *TestDarkBrushColor*) and you can also use this brushes (also colors with a prefix *...Color* at the end) in your project: 
- PrimaryVeryLightBrush
- PrimaryLightBrush
- PrimaryMidBrush
- PrimaryDarkBrush
- PrimaryVeryDarkBrush
- SecondaryLightBrush
- SecondaryMidBrush
- SecondaryDarkBrush
- ScrollBackgroundBrush
- ScrollForegroundBrush
- TextForegroundBrush

*ConfigureApplicationVisual* should be called in *ConfigureModuleCatalog* overrided method like this:
```cs
protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
{
    // should be called right here
    ConfigureApplicationVisual();

    base.ConfigureModuleCatalog(moduleCatalog);
    // ...
}
```
If you won't register any of the theme it will be gererated automaticaly by reversing colors of the existing theme.  
Registered colors and brushes could be used as DynamicResources like that:
```xaml
<Rectangle Fill="{DynamicResource TestMidBrush}"/>
```
