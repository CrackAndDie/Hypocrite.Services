using Hypocrite.Localization;
using Hypocrite.Services;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Services;
using Hypocrite.Core.Logging.Interfaces;
using Hypocrite.Core.Logging.Services;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;
using Prism;
using Hypocrite.Container;
using Hypocrite.Core.Events;
using Hypocrite.Container.Prism;

namespace Hypocrite.Mvvm
{
    public class ApplicationBase : PrismApplicationBase, IContainerHolder
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalizationManager.Initialize();
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            if (Container.IsRegistered<IPreviewWindow>())
            {
                var ev = Container.Resolve<IEventAggregator>();
                ev.GetEvent<PreviewDoneEvent>().Subscribe(OnPreviewDone);
                return Container.Resolve<IPreviewWindow>() as Window;
            }
            var window = Container.Resolve<IBaseWindow>() as Window;
            Application.Current.MainWindow = window;
            return window;
        }

        private void OnPreviewDone()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Container.Resolve<IBaseWindow>() as Window;
                Application.Current.MainWindow = window;
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
            return new HypocriteContainerExtension(container);
        }
    }
}
