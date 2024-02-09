using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Interfaces
{
    public interface IViewModel
    {
        object View { get; set; }
        /// <summary>
        /// Called after View is loaded
        /// </summary>
        void OnViewReady();
        /// <summary>
        /// Called after View in attached to the VM
        /// </summary>
        void OnViewAttached();
    }
}
