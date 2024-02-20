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
    }

    class ContainerTestClass2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
