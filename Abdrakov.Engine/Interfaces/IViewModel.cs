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
        /// <summary>
        /// Called after constructor and after View is assigned
        /// </summary>
        void OnDependenciesReady();
        /// <summary>
        /// Called after View is loaded
        /// </summary>
        void OnViewReady();
    }
}
