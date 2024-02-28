using Hypocrite.Core.Container.Interfaces;

namespace Hypocrite.Core.Container.InstancePolicy
{
    // also for instances
    public class SingletonPolicy : BasePolicy
    {
        public SingletonPolicy(ILightContainer lightContainer, IContainerRegistration registration) : base(lightContainer, registration)
        {
        }

        public override object CreateInstance(bool withInjections)
        {
            // if there is no need for injection
            if (!_registration.MemberInjectionInfo.RequireInjection)
                OnInjectionDone();

            // all the injections should be resolved in the instance on its first resolve
            bool reqInj = !_registration.MemberInjectionInfo.IsInjected && withInjections;
            if (_registration.Instance != null && reqInj)
                _lightContainer.InstanceCreator.ResolveInjections(_registration.Instance, _registration.MemberInjectionInfo);
            else if (_registration.Instance == null)
                _registration.Instance = _lightContainer.InstanceCreator.CreateInstance(_registration, reqInj);

            if (reqInj)
                OnInjectionDone();

            return _registration.Instance;
        }

        public override void OnInjectionDone()
        {
            _registration.MemberInjectionInfo.IsInjected = true;
        }
    }
}
