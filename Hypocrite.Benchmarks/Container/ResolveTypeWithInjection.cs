using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveTypeWithInjection
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public ResolveTypeWithInjection()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _lightContainer.RegisterType(typeof(ContainerTestClass3Light), typeof(ContainerTestClass3Light));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
            _unityContainer.RegisterType<ContainerTestClass3Unity>();
        }

        [Benchmark]
        public int WithUnityContainer()
        {
            return _unityContainer.Resolve<ContainerTestClass3Unity>().TestClass.Id;
        }

        [Benchmark]
        public int WithLightContainer()
        {
            return (_lightContainer.Resolve(typeof(ContainerTestClass3Light)) as ContainerTestClass3Light).TestClass.Id;
        }
    }
}
