using Abdrakov.Weaver.Interfaces;
using Abdrakov.Weaver.Resolvers;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Abdrakov.Weaver 
{
    public class WeavingTask : AppDomainIsolatedTask
    {
        private IWeaver _weaver;

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        private string assemblyPath;
        public string AssemblyPath
        {
            get { return assemblyPath; }
            set { assemblyPath = value; }
        }

        public override bool Execute()
        {
            Log.LogMessage("Start of weaving: ", AssemblyPath);

            _weaver = new Weaver();

            _weaver.RunOn(AssemblyPath, new List<IResolver>() { new ReactivePropertyResolver() });

            Log.LogMessage("End of weaving: ", AssemblyPath);

            return true;
        }
    }
}
