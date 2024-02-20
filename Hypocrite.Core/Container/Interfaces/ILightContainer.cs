using Hypocrite.Core.Container.Common;
using System;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface ILightContainer : IDisposable
    {
        QuickSet<IContainerRegistration> Registrations { get; }

        IInstanceCreator InstanceCreator { get; }

        void SetInstanceCreator(IInstanceCreator creator);

        ILightContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false);
        ILightContainer RegisterType(Type registeredType, Type mappedToType, string name, bool isSingleton = false);
        ILightContainer RegisterInstance(Type type, object instance);
        ILightContainer RegisterInstance(Type type, object instance, string name);
        ILightContainer RegisterFactory(Type type, Func<ILightContainer, Type, object> factory);

        IContainerRegistration GetRegistration(Type type, string name = "");

        bool IsRegistered(Type type);
        bool IsRegistered(Type type, string name);

        object Resolve(Type type);
        object Resolve(Type type, bool withInjections);
        object Resolve(Type type, string name, bool withInjections);
        object Resolve(Type type, string name, bool withInjections, out IContainerRegistration outRegistration);

        void ResolveInjections(object instance, MemberInjectionInfo injectionInfo = null);
        bool RequiresInjections(object instance, MemberInjectionInfo injectionInfo = null);
    }
}
