﻿using Hypocrite.DemoWpf.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Hypocrite.DemoWpf.Modules
{
    internal class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();
            // the view to display on start up
            region.RegisterViewWithRegion(Regions.MAIN_REGION, typeof(MainPageView));
            region.RegisterViewWithRegion(Regions.ADDITIONAL_REGION, typeof(AdditionalPageView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPageView>();
            containerRegistry.RegisterForNavigation<AdditionalPageView>();
        }
    }
}
