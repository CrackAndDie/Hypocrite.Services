using Hypocrite.Core.Container;
using Unity;

namespace Hypocrite.Benchmarks.Container
{
    public class ContainerTestClass3Light
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Injection]
        public ContainerTestClass2 TestClass { get; set; }
    }

    public class ContainerTestClass3Unity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Dependency]
        public ContainerTestClass2 TestClass { get; set; }
    }
}
