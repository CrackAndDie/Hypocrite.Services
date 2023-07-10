using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Interfaces
{
    public interface IWindowProgressService
    {
        void AddWaiter();
        void RemoveWaiter();
        void CallStateChangeEvent(bool isEmpty);
    }
}
