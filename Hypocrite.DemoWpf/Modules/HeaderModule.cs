﻿using Hypocrite.Views;
using Hypocrite.DemoWpf.Views;
using Hypocrite.DemoWpf.Views.HeaderViews;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.DemoWpf.Modules
{
    internal class HeaderModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();

            region.RegisterViewWithRegion(Regions.HEADER_RIGHT_REGION, typeof(RightControlView));
            region.RegisterViewWithRegion(Regions.HEADER_LEFT_REGION, typeof(LeftControlView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RightControlView>();
            containerRegistry.RegisterForNavigation<LeftControlView>();
        }
    }
}
