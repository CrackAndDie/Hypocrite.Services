using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Interfaces
{
    public interface IViewModel
    {
        object View { get; set; }
        void OnDependenciesReady();
        void OnViewReady();
    }
}
