using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Extensions;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class LightContainerSteps
    {
        LightContainer _lightContainer;

        public LightContainerSteps()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2), false);
        }

        [Benchmark]
        public void IsRegistered()
        {
            _lightContainer.IsRegistered(typeof(ContainerTestClass2));
        }

        [Benchmark]
        public void GetRegistration()
        {
            _lightContainer.GetRegistration(typeof(ContainerTestClass2), string.Empty);
        }

        // it takes about 60ns
        // var _ = reg.RegistrationPolicy.CreateInstance(true);

        [Benchmark]
        public void CreatePureInstance()
        {
            var reg = _lightContainer.GetRegistration(typeof(ContainerTestClass2), string.Empty);
            var _ = _lightContainer.InstanceCreator.CreatePureInstance(reg);
        }

        [Benchmark]
        public void CreateInstance()
        {
            var reg = _lightContainer.GetRegistration(typeof(ContainerTestClass2), string.Empty);
            var inst = _lightContainer.InstanceCreator.CreatePureInstance(reg);
            _lightContainer.InstanceCreator.ResolveInjections(inst, reg.MemberInjectionInfo);
        }
    }
}
