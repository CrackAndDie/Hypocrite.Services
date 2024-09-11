using Hypocrite.Mvvm;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Mvvm;
using Hypocrite.Core.Logging.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Windows;

namespace Hypocrite.Extensions
{
    public static class RegionManagerExtensions
    {
        public static void RequestNavigate<TView>(this IRegionManager regionManager, string regionName)
            where TView : IView
        {
            regionManager.DisposeAndRemoveExistingViews(regionName);
			regionManager.RequestNavigate(regionName, typeof(TView).Name, OnNavigationFinished);
        }

        public static void RequestNavigate<TView>(this IRegionManager regionManager, string regionName, object parameters)
            where TView : IView
        {
			regionManager.DisposeAndRemoveExistingViews(regionName);
			var navigationParametersType = typeof(GenericNavigationParameters<>).MakeGenericType(parameters.GetType());
            var constructor = navigationParametersType.GetConstructor(new Type[] { parameters.GetType() });
            regionManager.RequestNavigate(regionName, typeof(TView).Name, OnNavigationFinished, constructor.Invoke(new object[] { parameters }) as NavigationParameters);
        }

		public static void DisposeAndRemoveExistingViews(this IRegionManager regionManager, string regionName)
		{
			if (!regionManager.Regions.ContainsRegionWithName(regionName))
				return;

			var views = regionManager.Regions[regionName].Views as ViewsCollection;
			foreach (var view in views)
			{
				if (view is IDisposable)
					((IDisposable)view).Dispose();
				if (!(view is FrameworkElement))
					continue;
				var normalView = view as FrameworkElement;
				if (normalView.DataContext is IDisposable)
					((IDisposable)normalView.DataContext).Dispose();
			}
			regionManager.Regions[regionName].RemoveAll();
		}

		private static void OnNavigationFinished(NavigationResult result)
        {
            if (result.Error != null)
            {
                var loggingService = (Application.Current as ApplicationBase).Container.Resolve<ILoggingService>();
                loggingService.Error($"Navigation failed! Target: {result.Context.Uri}");

                throw result.Error;
            }
        }
    }
}
