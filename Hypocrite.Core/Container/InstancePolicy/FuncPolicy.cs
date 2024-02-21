using Hypocrite.Core.Container.Interfaces;

namespace Hypocrite.Core.Container.InstancePolicy
{
    internal class FuncPolicy : BasePolicy
    {
        public FuncPolicy(ILightContainer lightContainer, IContainerRegistration registration) : base(lightContainer, registration)
        {
        }

        public override object CreateInstance(bool withInjections)
        {
            return _registration.Instance;
        }

        public override void OnInjectionDone()
        {
            // nothing to do
        }
    }
}
