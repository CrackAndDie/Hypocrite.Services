using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveTypeWithCtorInjection
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public ResolveTypeWithCtorInjection()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _lightContainer.RegisterType(typeof(ContainerTestClass4Light), typeof(ContainerTestClass4Light));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
            _unityContainer.RegisterType<ContainerTestClass4Unity>();
        }

        [Benchmark]
        public int WithUnityContainer()
        {
            return _unityContainer.Resolve<ContainerTestClass4Unity>().TestClass.Id;
        }

        [Benchmark]
        public int WithLightContainer()
        {
            return (_lightContainer.Resolve(typeof(ContainerTestClass4Light)) as ContainerTestClass4Light).TestClass.Id;
        }
    }
}
