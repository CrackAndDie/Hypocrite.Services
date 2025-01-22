using Hypocrite.Localization;
using Hypocrite.Services;
using Hypocrite.Core.Container;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;
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
using Hypocrite.Container;
using Hypocrite.Core.Events;

namespace Hypocrite.Mvvm
{
    public class ApplicationBase : PrismApplicationBase, IContainerHolder
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
            else if (Container.IsRegistered<IPreviewView>())
            {
                Container.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Subscribe(OnPreviewDone);
                return Container.Resolve<IPreviewView>() as UserControl;
            }
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var window = Container.Resolve<IBaseWindow>() as Window;
                desktop.MainWindow = window;
                return window;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                var view = Container.Resolve<IBaseView>() as UserControl;
                singleViewPlatform.MainView = view;
                return view;
            }
            return null;
        }

        private void OnPreviewDone()
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var window = Container.Resolve<IBaseWindow>() as Window;
                    desktop.MainWindow = window;
                    window.Show();
                }
                else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                {
                    var view = Container.Resolve<IBaseView>() as UserControl;
                    singleViewPlatform.MainView = view;
                }
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
