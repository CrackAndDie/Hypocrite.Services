using System;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IInstanceCreator
    {
        object CreateInstance(IContainerRegistration registration, ILightContainer container, bool withInjections = true);
        void ResolveInjections(object instance, ILightContainer container, Type type = null);
        bool RequiresInjections(object instance, Type type = null);
    }
}
