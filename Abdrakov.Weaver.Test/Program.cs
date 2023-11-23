using Abdrakov.Weaver;
using Abdrakov.Weaver.Interfaces;
using Abdrakov.Weaver.Resolvers;

public class Program 
{
    public static void Main(string[] args)
    {
        string AssemblyPath = @"D:\kakish\Abdrakov.Solutions\Abdrakov.CommonWPF\bin\Debug\Abdrakov.CommonWPF.dll";
        IWeaver _weaver = new Weaver();

        _weaver.RunOn(AssemblyPath, new List<IResolver>() { new ReactivePropertyResolver() });
    }
}