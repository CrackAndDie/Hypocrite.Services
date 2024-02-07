using Hypocrite.Avalonia.Localization;
using Hypocrite.Avalonia.Services;
using Hypocrite.Core.Container;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Mvvm.Events;
using Hypocrite.Core.Services;
using Hypocrite.Core.Logging.Interfaces;
using Hypocrite.Core.Logging.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Services.Dialogs;
using Prism;
using Hypocrite.Avalonia.Container;

namespace Hypocrite.Avalonia.Mvvm
{
    public class AbdrakovApplication : PrismApplicationBase, IContainerHolder
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
            var container = new LightContainer();
            return new LightContainerExtension(container);
        }
    }
}
