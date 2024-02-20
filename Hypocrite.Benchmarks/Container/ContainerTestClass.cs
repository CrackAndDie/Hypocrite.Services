namespace Hypocrite.Benchmarks.Container
{
    public class ContainerTestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ContainerTestClass(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
