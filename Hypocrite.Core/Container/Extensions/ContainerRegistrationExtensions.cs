﻿using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Registration;
using System;

namespace Hypocrite.Core.Container.Extensions
{
    public static class ContainerRegistrationExtensions
    {
        public static object GetInstance(this IContainerRegistration registration, ILightContainer container, bool withInjections = true)
        {
            // this is for cached Type instances
            if (registration.Instance != null && registration.RegistrationType == RegistrationType.Type)
            {
                return registration.Instance;
            }

            if (registration.RegistrationType == RegistrationType.Instance)
            {
                // all the injections should be resolved in the instance on its first resolve
                if (!registration.MemberInjectionInfo.IsInjected && withInjections)
                {
                    if (registration.Instance != null)
                        container.InstanceCreator.ResolveInjections(registration.Instance, container, registration.MemberInjectionInfo);
                    else
                    {
                        registration.Instance = container.InstanceCreator.CreateInstance(registration, container, withInjections);
                    }
                    registration.MemberInjectionInfo.IsInjected = true;
                }
                return registration.Instance;
            }

            return container.InstanceCreator.CreateInstance(registration, container, withInjections);
        }

        public static Func<ILightContainer, Type, object> GetFunc(this IContainerRegistration registration)
        {
            return registration.Instance as Func<ILightContainer, Type, object>;
        }
    }
}
