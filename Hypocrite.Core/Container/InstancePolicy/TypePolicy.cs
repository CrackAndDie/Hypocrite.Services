using Hypocrite.Core.Container.Interfaces;

namespace Hypocrite.Core.Container.InstancePolicy
{
    public class TypePolicy : BasePolicy
    {
        public TypePolicy(ILightContainer lightContainer, IContainerRegistration registration) : base(lightContainer, registration)
        {
        }

        public override object CreateInstance(bool withInjections)
        {
            // check for cached instance
            if (_registration.Instance != null)
            {
                return _registration.Instance;
            }

            object instance = _lightContainer.InstanceCreator.CreatePureInstance(_registration);
            if (withInjections && _registration.MemberInjectionInfo.RequireInjection)
            {
                _registration.Instance = instance;
                _lightContainer.InstanceCreator.ResolveInjections(instance, _registration.MemberInjectionInfo);
                _registration.Instance = null;
            }

            return instance;
        }

        public override void OnInjectionDone()
        {
            // nothing to do
        }
    }
}
