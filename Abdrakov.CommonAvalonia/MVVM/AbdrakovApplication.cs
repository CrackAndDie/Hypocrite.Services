﻿using Abdrakov.CommonAvalonia.Localization;
using Abdrakov.CommonAvalonia.Services;
using Abdrakov.Container.AvaloniaPrismAdapter;
using Abdrakov.Container;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Mvvm.Events;
using Abdrakov.Engine.Services;
using Abdrakov.Logging.Interfaces;
using Abdrakov.Logging.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Services.Dialogs;

namespace Abdrakov.CommonAvalonia.Mvvm
{
    public class AbdrakovApplication : PrismApplication, IContainerHolder
    {
        public static bool IsSingleViewLifetime =>
            Environment.GetCommandLineArgs()
            .Any(a => a == "--fbdev" || a == "--drm");

        public override void Initialize()
        {
            // AvaloniaXamlLoader.Load(this);
            LocalizationManager.Initialize();
            base.Initialize();              // <-- Required
        }

        protected override AvaloniaObject CreateShell()
        {
            if (Container.IsRegistered<IPreviewWindow>())
            {
                Container.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Subscribe(OnPreviewDone);
                return Container.Resolve<IPreviewWindow>() as Window;
            }
            var window = Container.Resolve<IBaseWindow>() as Window;
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = window;
            }
            return window;
        }

        private void OnPreviewDone()
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var window = Container.Resolve<IBaseWindow>() as Window;
                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.MainWindow = window;
                }
                window.Show();
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        protected virtual void RegisterDefaults(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ILoggingService>(new Log4netLoggingService());
            containerRegistry.RegisterSingleton<IViewModelResolverService, ViewModelResolverService>();
            containerRegistry.RegisterSingleton<IWindowProgressService, WindowProgressService>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelResolver = Container.Resolve<IViewModelResolverService>();
                return viewModelResolver.ResolveViewModelType(viewType);
            });

            ViewModelLocationProvider.SetDefaultViewModelFactory((view, viewModelType) =>
            {
                var viewModelResolver = Container.Resolve<IViewModelResolverService>();
                return viewModelResolver.ResolveViewModel(viewModelType, view);
            });
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var container = new AbdrakovContainer();
            return new AbdrakovContainerExtension(container);
        }
    }
}
