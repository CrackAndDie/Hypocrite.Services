<p align="center">
  <a>
    <img src="https://raw.githubusercontent.com/CADindustries/container/main/logos/AbdrakovSolutions.png" alt="Abdrakov.Solutions logo" width="340" height="340">
  </a>
</p>
<h1 align="center">Hypocrite.Services</h1>  

**WPF**
[![Nuget](https://img.shields.io/nuget/v/Hypocrite.Services.svg)](http://nuget.org/packages/Hypocrite.Services)
[![Nuget](https://img.shields.io/nuget/dt/Hypocrite.Services.svg)](http://nuget.org/packages/Hypocrite.Services)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CrackAndDie/Hypocrite.Services/blob/main/LICENSE)  

**Avalonia**
[![Nuget](https://img.shields.io/nuget/v/Hypocrite.Services.Avalonia.svg)](http://nuget.org/packages/Hypocrite.Services.Avalonia)
[![Nuget](https://img.shields.io/nuget/dt/Hypocrite.Services.Avalonia.svg)](http://nuget.org/packages/Hypocrite.Services.Avalonia)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CrackAndDie/Hypocrite.Services/blob/main/LICENSE)

<h2>About:</h2>  

A package that helps You to create a powerful, flexible and loosely coupled WPF and Avalonia application. It fully supports Prism features and MVVM pattern.  

<h2>LightContainer</h2>  

<h3>Why should I use LightContainer?</h3>  

*Hypocrite.Services* provides You light and fast DI container called *LightContainer*. Here are some features of it:

<h4>Attribute injections:</h4>  

All the registered shite could be resolved via *Injection* attribute (use the attribute only for properties and fields) like this:
```c#
private class NormalClass
{
    [Injection]
    InjectedClass TestClass { get; set; }
    [Injection]
    AnotherInjectedClass _anotherTestClass;
}
```

<h4>Constructor injections:</h4>  

Parametrised constructors could also be used (this feature is not fully provided by *UnityContainer*). For example after registering and resolving the class:  
```c#
private class NormalClass
{
    private InjectedClass _testClass;
    private int _a;
    private string _b;

    public NormalClass(InjectedClass testClass, int a, string b = "awd")
    {
        _testClass = testClass;
        _a = a;
        _b = b;
    }
}
```
the *testClass* parameter would be resolved as usual (if it is not registered in the container then an instance of it would be created); the *a* parameter would have **default type** value (for Int32 is 0); the *b* parameter would have its **default parameter** value (in this case is "awd").  

<h4>Inheritance injections:</h4>  

The classed from which Your class is inherited would also be prepared for injections:  
```c#
private class InjectedClass
{
    internal int A { get; set; }
}

private class BaseClass
{
    [Injection]
    protected InjectedClass TestClass { get; set; }
}

private class NormalClass : BaseClass
{
}
```
So in this case after *NormalClass* registration and resolve, the *TestClass* property would also be injected.  

<h4>Recursive injections:</h4>  

There could be two classes that require injection of each other (this feature is not provided by *UnityContainer*):
```c#
private class FirstClass
{
    [Injection]
    SecondClass InjectedClass { get; set; }
}

private class SecondClass
{
    [Injection]
    FirstClass InjectedClass { get; set; }
}
```
And this would work as expected!  

<h3>What about speed?</h3>  

After some benchmarks on my shity laptop (idk and idc about its parameters) I got the following things:  

![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/9f6636f6-7a1d-464f-b795-29129cba3a7d)

Singleton resolve:  
![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/8e9e2df9-2632-4512-b5da-5a6f006e8861)  
Type resolve:  
![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/655b8ca1-2734-42c4-9d9f-eaff5e1d7d1a)  

But... there is a moment with injections cause they're quite powerful in *LightContainer*:  

![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/2ae3642f-889b-4131-9943-6b54e90a3b08)

Type resolve with constructor injections:  
![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/694c1808-ee27-4074-9a15-116335312210)  
Type resolve with fields and properties injections:  
![image](https://github.com/CrackAndDie/Hypocrite.Services/assets/52558686/8a0c8798-f1d5-4b4e-814e-1ffc93c92252)



<h2>Read more:</h2>  

Read more about how to use the library with [WPF](https://github.com/CrackAndDie/Hypocrite.Services/blob/main/README_Wpf.md) or [Avalonia](https://github.com/CrackAndDie/Hypocrite.Services/blob/main/README_Avalonia.md).
