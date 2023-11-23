using Abdrakov.Weaver.Interfaces;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Weaver
{
    public class Weaver : IWeaver
    {
        public void RunOn(string assembly, IList<IResolver> resolvers)
        {
            using (var module = ModuleDefinition.ReadModule(assembly, new ReaderParameters { ReadWrite = true, AssemblyResolver = new CustomResolver() }))
            {
                foreach (var resolver in resolvers)
                {
                    resolver.ModuleDefinition = module;
                    resolver.Execute();
                }

                module.Write(); // Write to the same file that was used to open the file
            }
        }

        private class CustomResolver : BaseAssemblyResolver
        {
            private DefaultAssemblyResolver _defaultResolver;

            public CustomResolver()
            {
                _defaultResolver = new DefaultAssemblyResolver();
            }

            public override AssemblyDefinition Resolve(AssemblyNameReference name)
            {
                AssemblyDefinition assembly;
                try
                {
                    assembly = _defaultResolver.Resolve(name);
                }
                catch (AssemblyResolutionException ex)
                {
                    assembly = _defaultResolver.Resolve(name);
                }
                return assembly;
            }
        }
    }
}
