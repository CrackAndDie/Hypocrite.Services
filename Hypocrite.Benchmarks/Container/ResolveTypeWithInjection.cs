using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Extensions;
using Hypocrite.Core.Container.Extensions;
using Ninject;
using StyletIoC;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveTypeWithInjection
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;
		IKernel _ninjectContainer;
		IContainer _styletContainer;

		public ResolveTypeWithInjection()
        {
            _lightContainer = new LightContainer();
            _lightContainer.RegisterType(typeof(ContainerTestClass2), typeof(ContainerTestClass2));
            _lightContainer.RegisterType(typeof(ContainerTestClass3Light), typeof(ContainerTestClass3Light));
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
            _unityContainer.RegisterType<ContainerTestClass3Unity>();
			_ninjectContainer = new StandardKernel();
			_ninjectContainer.Bind<ContainerTestClass2>().ToSelf();
			_ninjectContainer.Bind<ContainerTestClass3Ninject>().ToSelf();
			var builder = new StyletIoCBuilder();
			builder.Bind<ContainerTestClass2>().ToSelf();
			builder.Bind<ContainerTestClass3Stylet>().ToSelf();
			_styletContainer = builder.BuildContainer();
		}

        [Benchmark]
        public int WithUnityContainer()
        {
            return _unityContainer.Resolve<ContainerTestClass3Unity>().TestClass.Id;
        }

        [Benchmark]
        public int WithLightContainer()
        {
            return _lightContainer.Resolve<ContainerTestClass3Light>().TestClass.Id;
        }

		[Benchmark]
		public int WithNinjectContainer()
		{
			return _ninjectContainer.Get<ContainerTestClass3Ninject>().TestClass.Id;
		}

		[Benchmark]
		public int WithStyletContainer()
		{
			return _styletContainer.Get<ContainerTestClass3Stylet>().TestClass.Id;
		}
	}
}
