﻿using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hypocrite.Wpf.Mvvm
{
    public class ViewModelBase : EngineViewModelBase, INavigationAware, IRequireInjection
    {
        public ViewModelBase() 
        {
            Application = (System.Windows.Application.Current as AbdrakovApplication);
            LoggingService.Debug($"Constructing '{ViewModelName}'");
        }

        public override void OnViewAttached()
        {
            base.OnViewAttached();

            if (View is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded += (obj, args) => OnViewReady();
            }
        }

        public virtual void OnInjectionsReady()
        {
        }

        public virtual void OnResolveReady()
        {
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
