using Abdrakov.Container;
using Abdrakov.Container.Interfaces;
using Abdrakov.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Abdrakov.CommonAvalonia.Services
{
    public class ViewModelResolverService : IViewModelResolverService
    {
        public void RegisterViewModelAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
        }

        public object ResolveViewModel(Type viewModelType, object view)
        {
            var vm = Container.Resolve(viewModelType);
            if (vm is IViewModel viewModel)
            {
                viewModel.View = view;
                viewModel.OnDependenciesReady();
            }

            return vm;
        }

        public Type ResolveViewModelType(Type viewType)
        {
            Type viewModelType = null;
            foreach (var assembly in assemblies.Reverse())
            {
                viewModelType = ResolveViewModelTypeFromAssembly(viewType, assembly);
                if (viewModelType != null)
                {
                    return viewModelType;
                }
            }

            if (viewModelType == null)
            {
                viewModelType = ResolveViewModelTypeFromAssembly(viewType, viewType.Assembly);
            }

            return viewModelType;
        }

        private Type ResolveViewModelTypeFromAssembly(Type viewType, Assembly assembly)
        {
            var viewAssembly = viewType.Assembly;
            var viewAssemblyName = viewAssembly.GetName().Name;
            var viewModelAssemblyName = assembly.GetName().Name;
            var viewName = viewType.FullName;
            var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var viewModelName = viewName.Replace("Views", "ViewModels") + suffix;
            if (viewModelName.StartsWith($"{viewAssemblyName}."))
            {
                viewModelName = $"{viewModelAssemblyName}." + viewModelName.Substring($"{viewAssemblyName}.".Length);
            }

            var viewModelAssemblyFullName = assembly.FullName;
            var viewModelFullName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewModelName, viewModelAssemblyFullName);
            var viewModelType = Type.GetType(viewModelFullName);

            return viewModelType;
        }

        [Injection]
        public IAbdrakovContainer Container { get; set; }

        private readonly IList<Assembly> assemblies = new List<Assembly>();
    }
}
