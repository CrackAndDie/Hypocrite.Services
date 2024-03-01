using BenchmarkDotNet.Attributes;
using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Extensions;
using Unity;
using System.Collections.Generic;

namespace Hypocrite.Benchmarks.Container
{
    [MemoryDiagnoser]
    public class ResolveOverloadType
    {
        ILightContainer _lightContainer;
        IUnityContainer _unityContainer;

        public ResolveOverloadType()
        {
            _lightContainer = new LightContainer();
            _unityContainer = new UnityContainer();

            for (char c = 'A'; c <= 'Z'; ++c)
            {
                _lightContainer.RegisterType<ContainerTestClass2>(c.ToString());
                _unityContainer.RegisterType<ContainerTestClass2>(c.ToString());
            }
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
    }
}
