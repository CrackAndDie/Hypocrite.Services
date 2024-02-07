﻿using Hypocrite.Avalonia.Mvvm;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Logging.Interfaces;
using Avalonia;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hypocrite.Avalonia.Extensions
{
    public static class RegionManagerExtensions
    {
        public static void RequestNavigate<TView>(this IRegionManager regionManager, string regionName)
            where TView : IView
        {
            regionManager.RequestNavigate(regionName, typeof(TView).Name, OnNavigationFinished);
        }

        public static void RequestNavigate<TView>(this IRegionManager regionManager, string regionName, object parameters)
            where TView : IView
        {
            var navigationParametersType = typeof(GenericNavigationParameters<>).MakeGenericType(parameters.GetType());
            var constructor = navigationParametersType.GetConstructor(new Type[] { parameters.GetType() });
            regionManager.RequestNavigate(regionName, typeof(TView).Name, OnNavigationFinished, constructor.Invoke(new object[] { parameters }) as NavigationParameters);
        }

        private static void OnNavigationFinished(NavigationResult result)
        {
            if (result.Error != null)
            {
                var loggingService = (Application.Current as AbdrakovApplication).Container.Resolve<ILoggingService>();
                loggingService.Error($"Navigation failed! Target: {result.Context.Uri}");

                throw result.Error;
            }
        }
    }
}
