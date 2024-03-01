using Hypocrite.Core.Container;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    public class ContainerTestClass4Light
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContainerTestClass2 TestClass { get; set; }

        public ContainerTestClass4Light([Injection] ContainerTestClass2 testClass)
        {
            TestClass = testClass;
        }
    }

    public class ContainerTestClass4Unity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContainerTestClass2 TestClass { get; set; }

        public ContainerTestClass4Unity([Dependency] ContainerTestClass2 testClass)
        {
            TestClass = testClass;
        }
    }
}
