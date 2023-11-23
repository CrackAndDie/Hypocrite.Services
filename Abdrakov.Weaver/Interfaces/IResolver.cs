using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Weaver.Interfaces
{
    public interface IResolver
    {
        ModuleDefinition ModuleDefinition { get; set; }
        void Execute();
    }
}
