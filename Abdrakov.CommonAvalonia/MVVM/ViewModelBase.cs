using Abdrakov.Engine.MVVM;
using Avalonia.Controls;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.CommonAvalonia.MVVM
{
    public class ViewModelBase : EngineViewModelBase, INavigationAware
    {
        public ViewModelBase()
        {
            Application = (Avalonia.Application.Current as AbdrakovApplication);
            LoggingService.Debug($"Constructing '{ViewModelName}'");
        }

        public override void OnDependenciesReady()
        {
            base.OnDependenciesReady();

            if (View is Control frameworkElement)
            {
                frameworkElement.Loaded += (obj, args) => OnViewReady();
            }
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentNavigationParameters = navigationContext.Parameters;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

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

        public IRegionManager RegionManager => Container.Resolve<IRegionManager>();
        public IDialogService DialogService => Container.Resolve<IDialogService>();
        public NavigationParameters CurrentNavigationParameters { get; private set; }
    }
}
