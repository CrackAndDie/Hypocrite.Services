using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Weaver.Interfaces
{
    public interface IWeaver
    {
        void RunOn(string assembly, IList<IResolver> resolvers);
    }
}
