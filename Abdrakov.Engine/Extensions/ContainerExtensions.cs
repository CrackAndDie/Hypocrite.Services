using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Extensions
{
    public static class ContainerExtensions
    {
        public static void RegisterForNavigation<TFrom, TTo>(this IContainerRegistry containerRegistry)
            where TTo : class, TFrom
        {
            containerRegistry.Register<object, TTo>(typeof(TFrom).Name);
            containerRegistry.Register<TFrom, TTo>(typeof(TFrom).Name);
        }
    }
}
