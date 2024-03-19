using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Ninject;
using StyletIoC;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveType
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;
		IKernel _ninjectContainer;
		IContainer _styletContainer;

		public ResolveType()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
			_ninjectContainer = new StandardKernel();
			_ninjectContainer.Bind<ContainerTestClass2>().ToSelf();
			var builder = new StyletIoCBuilder();
			builder.Bind<ContainerTestClass2>().ToSelf();
			_styletContainer = builder.BuildContainer();
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

		[Benchmark]
		public ContainerTestClass2 WithNinjectContainer()
		{
			return _ninjectContainer.Get<ContainerTestClass2>();
		}

		[Benchmark]
		public ContainerTestClass2 WithStyletContainer()
		{
			return _styletContainer.Get<ContainerTestClass2>();
		}
	}
}
