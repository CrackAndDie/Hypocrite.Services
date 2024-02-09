using System;

namespace Hypocrite.Core.Utils
{
    internal static class Stubs
    {
        public static readonly Action Nop = () => { };
        public static readonly Action<Exception> Throw = ex => { throw ex; };
    }
}
