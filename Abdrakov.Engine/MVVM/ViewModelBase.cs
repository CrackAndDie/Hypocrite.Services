using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Logging.Interfaces;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Engine.MVVM
{
    public class ViewModelBase : BindableObject, IViewModel, INavigationAware
    {
        public ViewModelBase()
        {
            viewModelName = GetType().FullName;
            LoggingService.Debug($"Constructing '{ViewModelName}'");
        }

        public virtual void OnDependenciesReady()
        {
            LoggingService.Debug($"OnDependenciesReady '{ViewModelName}'");

            if (View is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded += (obj, args) => OnViewReady();
            }
        }

        public virtual void OnViewReady()
        {
            LoggingService.Debug($"OnViewReady '{ViewModelName}'");
            EventAggregator.GetEvent<NavigationEvent>().Publish(this);
        }

        public TView GetView<TView>()
             where TView : class
        {
            return View as TView;
        }

        protected ParametersType GetNavigationParameters<ParametersType>()
            where ParametersType : class, new()
        {
            var parameters = new ParametersType();
            foreach (var parameter in CurrentNavigationParameters)
            {
                var tt = typeof(ParametersType);
                var ff = tt.GetProperty(parameter.Key);
                if (ff != null)
                    ff.SetValue(parameters, parameter.Value);
            }

            return parameters;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentNavigationParameters = navigationContext.Parameters;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public AbdrakovApplication Application => System.Windows.Application.Current as AbdrakovApplication;
        public IContainerProvider Container => Application.Container;
        public IEventAggregator EventAggregator => Container.Resolve<IEventAggregator>();
        public IRegionManager RegionManager => Container.Resolve<IRegionManager>();
        public ILoggingService LoggingService => Container.Resolve<ILoggingService>();
        public IDialogService DialogService => Container.Resolve<IDialogService>();
        public IWindowProgressService WindowProgressService => Container.Resolve<IWindowProgressService>();
        public string ViewModelName => viewModelName;
        public NavigationParameters CurrentNavigationParameters { get; private set; }

        public object View { get; set; }

        protected readonly string viewModelName;
    }
}
