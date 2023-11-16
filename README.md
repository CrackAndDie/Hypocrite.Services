<p align="center">
  <a href="https://robocadsim.readthedocs.io/en/latest/index.html">
    <img src="https://raw.githubusercontent.com/CADindustries/container/main/logos/AbdrakovSolutions.png" alt="Abdrakov.Solutions logo" width="340" height="340">
  </a>
</p>
<h1 align="center">Abdrakov.Solutions</h1>  

[![Nuget](https://img.shields.io/nuget/v/Abdrakov.Solutions.svg)](http://nuget.org/packages/Abdrakov.Solutions)
[![Nuget](https://img.shields.io/nuget/dt/Abdrakov.Solutions.svg)](http://nuget.org/packages/Abdrakov.Solutions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CrackAndDie/Abdrakov.Solutions/blob/main/LICENSE)

<h2>About:</h2>  

A package that helps You to create a powerful, flexible and loosely coupled WPF application. It fully supports Prism features and MVVM pattern.  

<h2>Download:</h2>  

<h3>.NET CLI:</h3>  

```dotnet add package Abdrakov.Solutions```

<h3>Package Reference:</h3>  

```<PackageReference Include="Abdrakov.Solutions" Version="*" />```  

<h2>Demo:</h2>  

Demo could be downloaded from [releases](https://github.com/CrackAndDie/Abdrakov.Solutions/releases)  

<img src="https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/b35455e9-f6f4-4c3a-bd5a-b6b71dac4223" alt="image1" width="440">  
<img src="https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/db299c87-7bb2-4489-972c-3065708d0b24" alt="image2" width="440">  

<h2>Getting started:</h2>  

<h3>Topics:</h3>  

- [First steps](#first-steps)
- [Preview window](#preview-window)
- [Theme registrations](#theme-registrations)
- [Localization](#localization)
- [Window progress indicator](#window-progress-indicator)
- [Logging](#logging)

<h3>First steps:</h3>  

When you created your WPF app you should rewrite your *App.xaml* and *App.xaml.cs* files as follows:
```xaml
<engine:AbdrakovApplication x:Class="YourNamespace.App"
                            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                            xmlns:local="clr-namespace:YourNamespace"
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
                ProductName = "Abdrakov.Demo",
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
- *SmoothAppear* - How sould be window appeared (true - smooth, false - as usual)

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

Abdrakov.Solutions supports realtime theme change and flexible object registrations. Here is the sample code to register *ThemeSwitcherService* in your App (if you don't want the theme change service in your project you can skip this registration):
```cs
containerRegistry.RegisterInstance(new ThemeSwitcherService<Themes>()
{
    NameOfDictionary = "ThemeHolder",
    ThemeSources = new Dictionary<Themes, string>()
    {
        { Themes.Dark, "/Abdrakov.Demo;component/Resources/Themes/DarkTheme.xaml" },
        { Themes.Light, "/Abdrakov.Demo;component/Resources/Themes/LightTheme.xaml" },
    },
});
```

In my app *Themes* is an enum of themes for the app:
```c#
public enum Themes
{
    Dark,
    Light
}
```

Using the *ThemeSwitcherService* you can change an App's theme in realtime using method *ChangeTheme(theme)* and get current theme using method *GetCurrentTheme()*.   

For proper work of *ThemeSwitcherService* You should create *ResourceDictionaries* for each theme You have and a *ResourceDictionary* that will hold all the changes of themes. So in my project I created *DarkTheme.xaml*, *LightTheme.xaml* and *ThemeHolder.xaml*.  
*DarkTheme.xaml*:
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Color x:Key="TextForegroundBrushColor">AliceBlue</Color>
    <SolidColorBrush x:Key="TextForegroundBrush" 
                     Color="{DynamicResource TextForegroundBrushColor}"/>

    <Color x:Key="WindowStatusBrushColor">#2f80ed</Color>
    <SolidColorBrush x:Key="WindowStatusBrush"
                     Color="{DynamicResource WindowStatusBrushColor}" />

    <Color x:Key="WindowBrushColor">#070c13</Color>
    <SolidColorBrush x:Key="WindowBrush"
                     Color="{DynamicResource WindowBrushColor}" />
</ResourceDictionary>
```
*LightTheme.xaml*:
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Color x:Key="TextForegroundBrushColor">Black</Color>
    <SolidColorBrush x:Key="TextForegroundBrush"
                     Color="{DynamicResource TextForegroundBrushColor}" />
    
    <Color x:Key="WindowStatusBrushColor">#2f80ed</Color>
    <SolidColorBrush x:Key="WindowStatusBrush"
                     Color="{DynamicResource WindowStatusBrushColor}" />
    
    <Color x:Key="WindowBrushColor">#fefefe</Color>
    <SolidColorBrush x:Key="WindowBrush"
                     Color="{DynamicResource WindowBrushColor}" />
</ResourceDictionary>
```
*ThemeHolder.xaml* (You should set here a default theme):
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/Abdrakov.Demo;component/Resources/Themes/DarkTheme.xaml"/>
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

Here are the colors and brushes that should be registered in your app:
- *TextForegroundBrush(Color)* - The colors of the product name and window buttons
- *WindowStatusBrush(Color)* - The colors of the window progress indicator
- *WindowBrush(Color)* - The colors of the window and window header backgrounds

The holder of *.xaml* theme files (*ThemeHolder.xaml*) should be merged into Your app resources like this:
```xaml
<engine:AbdrakovApplication x:Class="Abdrakov.Demo.App"
                            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                            xmlns:local="clr-namespace:Abdrakov.Demo"
                            xmlns:engine="clr-namespace:Abdrakov.Engine.MVVM;assembly=Abdrakov.Engine">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/Abdrakov.Demo;component/Resources/Themes/ThemeHolder.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</engine:AbdrakovApplication>
```
  
Registered colors and brushes could be used as DynamicResources like that:
```xaml
<Rectangle Fill="{DynamicResource TestBrush}"/>
```

There is also a *ThemeChangedEvent* event that is called through *IEventAggregator* with *ThemeChangedEventArgs*. You can subscribe like this (*IEventAggregator* is already resolved in *ViewModelBase*):
```c#
(Application.Current as AbdrakovApplication).Container.Resolve<IEventAggregator>().GetEvent<ThemeChangedEvent>().Subscribe(YourMethod);
```

<h3>Localization:</h3>  

*Abdrakov.Solutions* also provides you a great realtime localization solution. To use it you should create a folder *Localization* in your project and make some changes in your *App.xaml.cs* file:
```cs
public partial class App : AbdrakovApplication
{
    protected override void OnStartup(StartupEventArgs e)
    {
        LocalizationManager.InitializeExternal(Assembly.GetExecutingAssembly(), new ObservableCollection<Language>()
        {
            new Language() { Name = "EN" },
            new Language() { Name = "RU" },
        });  // initialization of LocalizationManager static service
        // ...
        base.OnStartup(e);
    }
}
```

In the *Localization* folder you create Resource files with translations and call it as follows - "FileName"."Language".resx (Gui.resx or Gui.ru.resx). Default resource file doesn't need to have the "Language" part.  

Now you can use it like this:
```xaml
<TextBlock Text="{LocalizedResource MainPage.TestText}"
            Foreground="{DynamicResource TextForegroundBrush}" />
```
Or via *Binding*s:
```xaml
<TextBlock Text="{LocalizedResource {Binding TestText}}"
            Foreground="{DynamicResource TextForegroundBrush}" />
```
(I have this in my *.resx* files):  
![image](https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/d7e8969d-1f9e-4f2d-938a-d6e9c483f16c)
![image](https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/56e85807-d49f-448e-8fe1-f49d0b4158ce)

To change current localization you can change *LocalizationManager.CurrentLanguage* property like this:
```cs
LocalizationManager.CurrentLanguage = CultureInfo.GetCultureInfo(lang.Name.ToLower());
```
In this examle *lang* is an instance of *Language* class.  

<h3>Window progress indicator:</h3>  

There is also a progress indicator on the *MainWindowView* header that could be used to show user current window status. To handle this status you can resolve *IWindowProgressService* service (or use *WindowProgressService* property of *ViewModelBase*) and use *AddWaiter()* method to add waiter to the service and *RemoveWaiter()* when the job is done. You can also handle *WindowProgressChangedEvent* by yourself using *IEventAggregator*.  

<h3>Logging:</h3>  

To log your app's work you can resolve *ILoggingService* that is just an adapter of *Log4netLoggingService* or use *LoggingService* property of *ViewModelBase*.  

You can find the log file in you running assembly directory called *cadlog.log*.

<h2>Powered by:</h2>  

- *Abdrakov.Solutions*' logo - [Regina Danilkina](https://www.behance.net/reginadanilkina)
- [Prism](https://github.com/PrismLibrary/Prism)
- [log4net](https://github.com/apache/logging-log4net)
- *Abdrakov.Engine.Localization* is a rewritten part of [WPFLocalizeExtension](https://github.com/XAMLMarkupExtensions/WPFLocalizeExtension)
