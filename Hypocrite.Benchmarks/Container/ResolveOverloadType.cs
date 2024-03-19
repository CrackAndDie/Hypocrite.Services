using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Extensions;
using Unity;
using System.Collections.Generic;
using Ninject;
using StyletIoC;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveOverloadType
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;
        IKernel _ninjectContainer;
        IContainer _styletContainer;

        public ResolveOverloadType()
        {
            _lightContainer = new LightContainer();
            _unityContainer = new UnityContainer();
			_ninjectContainer = new StandardKernel();
			var builder = new StyletIoCBuilder();

			for (char c = 'A'; c <= 'Z'; ++c)
            {
                _lightContainer.RegisterType<ContainerTestClass2>(c.ToString());
                _unityContainer.RegisterType<ContainerTestClass2>(c.ToString());
                _ninjectContainer.Bind<ContainerTestClass2>().ToSelf().Named(c.ToString());
				builder.Bind<ContainerTestClass2>().ToSelf().WithKey(c.ToString());
			}

			_styletContainer = builder.BuildContainer();
		}

        [Benchmark]
        public ContainerTestClass2 WithUnityContainer()
        {
            return _unityContainer.Resolve<ContainerTestClass2>("K");
        }

        [Benchmark]
        public ContainerTestClass2 WithLightContainer()
        {
            return _lightContainer.Resolve<ContainerTestClass2>("K");
        }

		[Benchmark]
		public ContainerTestClass2 WithNinjectContainer()
		{
			return _ninjectContainer.Get<ContainerTestClass2>("K");
		}

		[Benchmark]
		public ContainerTestClass2 WithStyletContainer()
		{
			return _styletContainer.Get<ContainerTestClass2>("K");
		}
	}
}
