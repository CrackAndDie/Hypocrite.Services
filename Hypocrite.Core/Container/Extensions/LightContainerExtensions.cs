using Hypocrite.Core.Container.Interfaces;
using System;

namespace Hypocrite.Core.Container.Extensions
{
    public static class LightContainerExtensions
    {
        public static ILightContainer RegisterType<T>(this ILightContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(T), typeof(T), false);
        }

        public static ILightContainer RegisterType<T>(this ILightContainer container, string name)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(T), typeof(T), name, false);
        }

        public static ILightContainer RegisterType<TFrom, TTo>(this ILightContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TFrom), typeof(TTo), false);
        }

        public static ILightContainer RegisterType<TFrom, TTo>(this ILightContainer container, string name)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TFrom), typeof(TTo), name, false);
        }

        public static ILightContainer RegisterSingleton<T>(this ILightContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(T), typeof(T), true);
        }

        public static ILightContainer RegisterSingleton<T>(this ILightContainer container, string name)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(T), typeof(T), name, true);
        }

        public static ILightContainer RegisterSingleton<TFrom, TTo>(this ILightContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TFrom), typeof(TTo), true);
        }

        public static ILightContainer RegisterSingleton<TFrom, TTo>(this ILightContainer container, string name)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).RegisterType(typeof(TFrom), typeof(TTo), name, true);
        }

		public static ILightContainer RegisterInstance<T>(this ILightContainer container, object instance)
		{
			return (container ?? throw new ArgumentNullException(nameof(container))).RegisterInstance(typeof(T), instance);
		}

		public static ILightContainer RegisterInstance<T>(this ILightContainer container, object instance, string name)
		{
			return (container ?? throw new ArgumentNullException(nameof(container))).RegisterInstance(typeof(T), instance, name);
		}

		public static bool IsRegistered<T>(this ILightContainer container)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).IsRegistered(typeof(T), string.Empty);
        }

        public static bool IsRegistered<T>(this ILightContainer container, string name)
        {
            return (container ?? throw new ArgumentNullException(nameof(container))).IsRegistered(typeof(T), name);
        }

        public static T Resolve<T>(this ILightContainer container)
        {
            return (T)(container ?? throw new ArgumentNullException(nameof(container))).Resolve(typeof(T));
        }

        public static T Resolve<T>(this ILightContainer container, string name)
        {
            return (T)(container ?? throw new ArgumentNullException(nameof(container))).Resolve(typeof(T), name, true);
        }
    }
}
