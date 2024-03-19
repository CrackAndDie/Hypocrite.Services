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

	public class ContainerTestClass3Ninject
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[Ninject.Inject]
		public ContainerTestClass2 TestClass { get; set; }
	}

	public class ContainerTestClass3Stylet
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[StyletIoC.Inject]
		public ContainerTestClass2 TestClass { get; set; }
	}
}
