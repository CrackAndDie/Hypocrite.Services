using Hypocrite.Core.Container.Interfaces;

namespace Hypocrite.Core.Container.InstancePolicy
{
    public abstract class BasePolicy
    {
        protected ILightContainer _lightContainer;
        protected IContainerRegistration _registration;

        protected BasePolicy(ILightContainer lightContainer, IContainerRegistration registration)
        {
            _lightContainer = lightContainer;
            _registration = registration;
        }

        public abstract object CreateInstance(bool withInjections);
        public abstract void OnInjectionDone();
    }
}
