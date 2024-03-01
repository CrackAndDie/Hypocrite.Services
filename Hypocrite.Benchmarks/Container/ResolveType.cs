using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveType
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public ResolveType()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
        }

        [Benchmark]
        public ContainerTestClass2 WithUnityContainer()
        {
            return _unityContainer.Resolve<ContainerTestClass2>();
        }

        [Benchmark]
        public ContainerTestClass2 WithLightContainer()
        {
            return _lightContainer.Resolve(typeof(ContainerTestClass2)) as ContainerTestClass2;
        }
    }
}
