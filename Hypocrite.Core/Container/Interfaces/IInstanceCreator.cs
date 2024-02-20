using Hypocrite.Core.Container.Common;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IInstanceCreator
    {
        object CreateInstance(IContainerRegistration registration, ILightContainer container, bool withInjections = true);
        void ResolveInjections(object instance, ILightContainer container, MemberInjectionInfo injectionInfo);
        bool RequiresInjections(object instance, MemberInjectionInfo injectionInfo);
    }
}
