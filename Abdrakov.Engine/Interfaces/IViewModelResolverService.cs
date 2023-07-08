using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Interfaces
{
    public interface IViewModelResolverService
    {
        object ResolveViewModel(Type viewModelType, object view);
        Type ResolveViewModelType(Type viewType);
        void RegisterViewModelAssembly(Assembly assembly);
    }
}
