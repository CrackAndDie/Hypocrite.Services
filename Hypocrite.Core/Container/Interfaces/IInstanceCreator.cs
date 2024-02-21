using Hypocrite.Core.Container.Common;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IInstanceCreator
    {
        object CreateInstance(IContainerRegistration registration, bool withInjections = true);
        object CreatePureInstance(IContainerRegistration registration);
        void ResolveInjections(object instance, MemberInjectionInfo injectionInfo);
        bool RequiresInjections(object instance, MemberInjectionInfo injectionInfo);
    }
}
