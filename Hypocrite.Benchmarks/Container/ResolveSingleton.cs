using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using System.Linq.Expressions;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveSingleton
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public ResolveSingleton() 
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2), true);
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterSingleton<ContainerTestClass2>();
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
