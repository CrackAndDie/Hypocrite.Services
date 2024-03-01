using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class IsRegistered
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public IsRegistered()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
        }

        [Benchmark]
        public bool WithUnityContainer()
        {
            return _unityContainer.IsRegistered<ContainerTestClass2>();
        }

        [Benchmark]
        public bool WithLightContainer()
        {
            return _lightContainer.IsRegistered(typeof(ContainerTestClass2));
        }
    }
}
