using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Registration;
using System;

namespace Hypocrite.Core.Container.Extensions
{
    public static class ContainerRegistrationExtensions
    {
        public static object GetInstance(this IContainerRegistration registration, ILightContainer container, bool withInjections = true)
        {
            if (registration.Instance != null && registration.RegistrationType == RegistrationType.Instance)
            {
                // all the injections should be resolved in the instance on its first resolve
                if (container.InstanceCreator.RequiresInjections(registration.Instance) && withInjections)
                {
                    container.InstanceCreator.ResolveInjections(registration.Instance, container);
                }
                if (registration.Instance is IRequireInjection reqInj2) reqInj2.OnResolveReady(); // callback
                return registration.Instance;
            }

            var instance = container.InstanceCreator.CreateInstance(registration, container, withInjections);
            if (registration.RegistrationType == RegistrationType.Instance)
            {
                registration.Instance = instance;
            }
            if (instance is IRequireInjection reqInj) reqInj.OnResolveReady(); // callback
            return instance;
        }

        public static Func<ILightContainer, Type, object> GetFunc(this IContainerRegistration registration)
        {
            return registration.Instance as Func<ILightContainer, Type, object>;
        }
    }
}
