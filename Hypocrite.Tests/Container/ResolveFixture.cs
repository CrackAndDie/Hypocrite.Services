using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Extensions;
using Unity;
using Xunit;

namespace Hypocrite.Tests.Container
{
    public class ResolveFixture
    {
        [Fact]
        public void UnityTypeResolvesAreDifferent()
        {
            var _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();

            var t1 = _unityContainer.Resolve<ContainerTestClass2>();
            var t2 = _unityContainer.Resolve<ContainerTestClass2>();

            t1.Id = 4;
            t2.Id = 5;

            Assert.NotEqual(t1.Id, t2.Id);
        }

        [Fact]
        public void LightTypeResolvesAreDifferent()
        {
            var _lightContainer = new LightContainer();
            _lightContainer.RegisterType<ContainerTestClass2>();

            var t1 = _lightContainer.Resolve<ContainerTestClass2>();
            var t2 = _lightContainer.Resolve<ContainerTestClass2>();

            t1.Id = 4;
            t2.Id = 5;

            Assert.NotEqual(t1.Id, t2.Id);
        }

        [Fact]
        public void UnityTypeResolvesInjectionsAreDifferent()
        {
            var _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass2>();
            _unityContainer.RegisterType<ContainerTestClass3Unity>();

            var t1 = _unityContainer.Resolve<ContainerTestClass3Unity>();
            var t2 = _unityContainer.Resolve<ContainerTestClass3Unity>();

            t1.TestClass.Id = 4;
            t2.TestClass.Id = 5;

            Assert.NotEqual(t1.TestClass.Id, t2.TestClass.Id);
        }

        [Fact]
        public void LightTypeResolvesInjectionsAreDifferent()
        {
            var _lightContainer = new LightContainer();
            _lightContainer.RegisterType<ContainerTestClass2>();
            _lightContainer.RegisterType<ContainerTestClass3Light>();

            var t1 = _lightContainer.Resolve<ContainerTestClass3Light>();
            var t2 = _lightContainer.Resolve<ContainerTestClass3Light>();

            t1.TestClass.Id = 4;
            t2.TestClass.Id = 5;

            Assert.NotEqual(t1.TestClass.Id, t2.TestClass.Id);
        }

        [Fact(Skip = "Stack overflow")]
        public void UnityTypeResolvesRecursiveTwice()
        {
            var _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ContainerTestClass4Light>();
            _unityContainer.RegisterType<ContainerTestClass5Unity>();

            var t1 = _unityContainer.Resolve<ContainerTestClass4Unity>();

            Assert.NotNull(t1.TestClass);
            Assert.NotNull(t1.TestClass.TestClass);
            Assert.NotNull(t1.TestClass.TestClass.TestClass);
            Assert.Equal(t1.TestClass, t1.TestClass.TestClass.TestClass);
        }

        [Fact]
        public void LightTypeResolvesRecursiveTwice()
        {
            var _lightContainer = new LightContainer();
            _lightContainer.RegisterType<ContainerTestClass4Light>();
            _lightContainer.RegisterType<ContainerTestClass5Light>();

            var t1 = _lightContainer.Resolve<ContainerTestClass4Light>();

            Assert.NotNull(t1.TestClass);
            Assert.NotNull(t1.TestClass.TestClass);
            Assert.NotNull(t1.TestClass.TestClass.TestClass);
            Assert.Equal(t1.TestClass, t1.TestClass.TestClass.TestClass);
        }
    }

    class ContainerTestClass2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class ContainerTestClass3Unity
    {
        [Dependency]
        public ContainerTestClass2 TestClass { get; set; }
    }

    class ContainerTestClass3Light
    {
        [Injection]
        public ContainerTestClass2 TestClass { get; set; }
    }

    class ContainerTestClass4Unity
    {
        [Dependency]
        public ContainerTestClass5Unity TestClass { get; set; }
    }

    class ContainerTestClass4Light
    {
        [Injection]
        public ContainerTestClass5Light TestClass { get; set; }
    }

    class ContainerTestClass5Unity
    {
        [Dependency]
        public ContainerTestClass4Unity TestClass { get; set; }
    }

    class ContainerTestClass5Light
    {
        [Injection]
        public ContainerTestClass4Light TestClass { get; set; }
    }
}
