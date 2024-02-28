<h1>Hypocrite.Services for WPF</h1>

<h2>Download:</h2>  

<h3>.NET CLI:</h3>  

```dotnet add package Hypocrite.Services```

<h3>Package Reference:</h3>  

```<PackageReference Include="Hypocrite.Services" Version="*" />```  

<h2>Demo:</h2>  

Demo could be downloaded from [releases](https://github.com/CrackAndDie/Hypocrite.Services/releases)  

<img src="https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/b35455e9-f6f4-4c3a-bd5a-b6b71dac4223" alt="image1" width="440">  
<img src="https://github.com/CrackAndDie/Abdrakov.Solutions/assets/52558686/db299c87-7bb2-4489-972c-3065708d0b24" alt="image2" width="440">  

<h2>Getting started:</h2>  

<h3>Topics:</h3>  

- [First steps](#first-steps)
- [Preview window](#preview-window)
- [Theme registrations](#theme-registrations)
- [Localization](#localization)
- [Logging](#logging)
- [Observables and Observers](#observables-and-observers)

<h3>First steps:</h3>  

When you created your WPF app you should rewrite your *App.xaml* and *App.xaml.cs* files as follows:
```xaml
<mvvm:ApplicationBase x:Class="YourNamespace.App"
                      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                      xmlns:local="clr-namespace:YourNamespace"
                      xmlns:mvvm="clr-namespace:Hypocrite.Mvvm;assembly=Hypocrite.Wpf">
</mvvm:ApplicationBase>
```

```cs
namespace YourNamespace
{
    public partial class App : ApplicationBase
    {
        protected override Window CreateShell()
        {
            var viewModelService = Container.Resolve<IViewModelResolverService>();
            viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());
            // ...

            return base.CreateShell();
        }
    }
}
```

<h3>Preview window:</h3>  

You can create your own preview window using *Hypocrite.Services*. Your preview window has to implement *IPreviewWindow* interface. Here is the sample preview window that shows up for 4 seconds:
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
        EventAggregator.GetEvent<PreviewDoneEvent>().Publish();
        this.Close();
    }

    [Injection]
    IEventAggregator EventAggregator { get; set; }
}
```

Registered Window under *IBaseWindow* interface will be shown up after *PreviewDoneEvent* event call.

<h3>Theme registrations:</h3>  

*Hypocrite.Services* supports realtime theme change and flexible object registrations. Here is the sample code to register *ThemeSwitcherService* in your App (if you don't want the theme change service in your project you can skip this registration):
```cs
containerRegistry.RegisterInstance(new ThemeSwitcherService<ThemeType>()
{
    NameOfDictionary = "ThemeHolder",
    ThemeSources = new Dictionary<ThemeType, string>()
    {
        { ThemeType.Dark, "/Hypocrite.DemoWpf;component/Resources/Themes/DarkTheme.xaml" },
        { ThemeType.Light, "/Hypocrite.DemoWpf;component/Resources/Themes/LightTheme.xaml" },
    },
});
```

In my app *ThemeType* is an enum of themes for the app:
```c#
public enum ThemeType
{
    Dark,
    Light
}
```

Using the *ThemeSwitcherService* you can change an App's theme in realtime using method *ChangeTheme(theme)* and get current theme using method *GetCurrentTheme()*.   

For proper work of *ThemeSwitcherService* You should create *ResourceDictionaries* for each theme You have and a *ResourceDictionary* that will hold all the changes of themes. So in my project I've created *DarkTheme.xaml*, *LightTheme.xaml* and *ThemeHolder.xaml*.  
*DarkTheme.xaml*:
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Color x:Key="TextForegroundBrushColor">AliceBlue</Color>
    <SolidColorBrush x:Key="TextForegroundBrush" 
                     Color="{DynamicResource TextForegroundBrushColor}"/>

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
        <ResourceDictionary Source="/Hypocrite.DemoWpf;component/Resources/Themes/DarkTheme.xaml"/>
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

The holder of *.xaml* theme files (*ThemeHolder.xaml*) should be merged into Your app resources like this:
```xaml
<mvvm:ApplicationBase x:Class="YourNamespace.App"
                      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                      xmlns:local="clr-namespace:YourNamespace"
                      xmlns:mvvm="clr-namespace:Hypocrite.Mvvm;assembly=Hypocrite.Wpf">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/Hypocrite.DemoWpf;component/Resources/Themes/ThemeHolder.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</mvvm:ApplicationBase>
```
  
Registered colors and brushes could be used as DynamicResources like that:
```xaml
<Rectangle Fill="{DynamicResource TestBrush}"/>
```

There is also a *ThemeChangedEvent* event that is called through *IEventAggregator* with *ThemeChangedEventArgs*. You can subscribe like this (*IEventAggregator* is already resolved in *ViewModelBase*):
```c#
EventAggregator.GetEvent<ThemeChangedEvent>().Subscribe(YourHandler);
```

<h3>Localization:</h3>  

*Hypocrite.Services* also provides you a great realtime localization solution. To use it you should create a folder *Localization* in your project and make some changes in your *App.xaml.cs* file:
```cs
public partial class App : ApplicationBase
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

To change current localization use *LocalizationManager.CurrentLanguage* property like this:
```cs
LocalizationManager.CurrentLanguage = CultureInfo.GetCultureInfo(lang.Name.ToLower());
```
In this examle *lang* is an instance of *Language* class.  

<h3>Logging:</h3>  

To log your app's work you can resolve *ILoggingService* that is just an adapter of *Log4netLoggingService* or use *LoggingService* property of *ViewModelBase*.  

You can find the log file in you running assembly directory called *app.log* (or set your own but this requires manual registration of the service).  

<h3>Observables and Observers:</h3>  

*Hypocrite.Services* provides You some methods to observe property changes in a bindable class. An example:
```c#
this.WhenPropertyChanged(x => x.BindableBrush).Subscribe((b) =>
{
    Debug.WriteLine($"Current brush is: {b}");
});
```  
Where *BindableBrush* is declared as:
```c#
[Notify]
public SolidColorBrush BindableBrush { get; set; }
```  
You should use [DynamicData](https://github.com/reactivemarbles/DynamicData) or [ReactiveUI](https://github.com/reactiveui/ReactiveUI) that provide more powerful work with *Observer* pattern.

<h2>Powered by:</h2>  

- *Hypocrite.Services*' logo - [Regina Danilkina](https://www.behance.net/reginadanilkina)
- [Prism](https://github.com/PrismLibrary/Prism)
- [log4net](https://github.com/apache/logging-log4net)
