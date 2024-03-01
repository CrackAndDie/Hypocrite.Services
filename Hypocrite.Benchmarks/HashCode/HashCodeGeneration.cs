using BenchmarkDotNet.Attributes;
using Hypocrite.Benchmarks.Container;

namespace Hypocrite.Benchmarks.HashCode
{
    [MemoryDiagnoser]
    public class HashCodeGeneration
    {
        HashTestClass hashTest1;
        HashTestClass2 hashTest2;
        HashTestClass3 hashTest3;

        public HashCodeGeneration()
        {
            hashTest1 = new HashTestClass();
            hashTest2 = new HashTestClass2();
            hashTest3 = new HashTestClass3();
        }

        [Benchmark]
        public int DefaultHashCode()
        {
            return hashTest1.GetHashCode();
        }

        [Benchmark]
        public int FNVHashCode()
        {
            return hashTest2.GetHashCode();
        }

        [Benchmark]
        public int QuickSetFNVHashCode()
        {
            return hashTest3.GetHashCode();
        }
    }

    class HashTestClass
    {
        public Type RegType { get; set; }
        public string Name { get; set; }

        public HashTestClass()
        {
            RegType = typeof(ContainerTestClass);
            Name = "CTestClassHashName";
        }
    }

    class HashTestClass2
    {
        public Type RegType { get; set; }
        public string Name { get; set; }

        public HashTestClass2()
        {
            RegType = typeof(ContainerTestClass);
            Name = "CTestClassHashName";
        }

        public override int GetHashCode()
        {
            int hash = (int)21661361;
            // Suitable nullity checks etc, of course :)
            hash = (hash * 16777619) ^ RegType.GetHashCode();
            hash = (hash * 16777619) ^ Name.GetHashCode();
            return hash;
        }
    }

    class HashTestClass3
    {
        public Type RegType { get; set; }
        public string Name { get; set; }

        public HashTestClass3()
        {
            RegType = typeof(ContainerTestClass);   
            Name = "CTestClassHashName";
        }

        public override int GetHashCode()
        {
            int hash = 0x7FFFFFFF;
            // Suitable nullity checks etc, of course :)
            hash = hash & RegType.AssemblyQualifiedName.GetHashCode();
            hash = hash & Name.GetHashCode();
            return hash;
        }
    }
}
