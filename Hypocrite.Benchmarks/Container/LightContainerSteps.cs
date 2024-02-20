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
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2), true);
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

        [Benchmark]
        public void GetConstructor()
        {
            var _ = typeof(ContainerTestClass2).GetNormalConstructor();
        }

        [Benchmark]
        public void CreateInstancePure()
        {
            var constructor = typeof(ContainerTestClass2).GetNormalConstructor();
            var _ = constructor.Invoke(null);
        }

        [Benchmark]
        public void CreateInstance()
        {
            var reg = _lightContainer.GetRegistration(typeof(ContainerTestClass2), string.Empty);
            var _ = _lightContainer.InstanceCreator.CreateInstance(reg, _lightContainer, true);
        }
    }
}
