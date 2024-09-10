using Prism.Ioc;

namespace Hypocrite.Core.Extensions
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
