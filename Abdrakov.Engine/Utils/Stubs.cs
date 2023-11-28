using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.Engine.Utils
{
    internal static class Stubs
    {
        public static readonly Action Nop = () => { };
        public static readonly Action<Exception> Throw = ex => { throw ex; };
    }
}
