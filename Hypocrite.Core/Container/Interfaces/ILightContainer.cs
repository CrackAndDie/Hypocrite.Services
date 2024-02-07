using System;
using System.Collections.Generic;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface ILightContainer : IDisposable
    {
        IList<IContainerRegistration> Registrations { get; }

        IInstanceCreator InstanceCreator { get; }

        void SetInstanceCreator(IInstanceCreator creator);

        ILightContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false);
        ILightContainer RegisterType(Type registeredType, Type mappedToType, string name, bool isSingleton = false);
        ILightContainer RegisterInstance(Type type, object instance);
        ILightContainer RegisterInstance(Type type, object instance, string name);
        ILightContainer RegisterFactory(Type type, Func<ILightContainer, Type, object> factory);

        bool IsRegistered(Type type);
        bool IsRegistered(Type type, string name);

        object Resolve(Type type);
        object Resolve(Type type, bool withInjections);
        object Resolve(Type type, string name, bool withInjections);
        void ResolveInjections(Type type);
    }
}
