using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Interfaces
{
    public interface IContainerHolder
    {
        IContainerProvider Container { get; }
    }
}
