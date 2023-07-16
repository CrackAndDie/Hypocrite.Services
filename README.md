<p align="center">
  <a href="https://robocadsim.readthedocs.io/en/latest/index.html">
    <img src="https://raw.githubusercontent.com/CADindustries/container/main/logos/AbdrakovSolutions.png" alt="Abdrakov.Solutions logo" width="340" height="340">
  </a>
</p>
<h1 align="center">Abdrakov.Solutions</h1>

<h2>About:</h2>  

This is the main repo for all the Abdrakov projects. This project fully supports Prism features and MVVM pattern. 

<h2>Getting started:</h2>  
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
