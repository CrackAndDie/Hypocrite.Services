using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Interfaces
{
    /// <summary>
    /// Service to control current theme and theme switches of an application
    /// </summary>
    public interface IThemeSwitcherService<T>
    {
        /// <summary>
        /// Name of the main resource dictionary
        /// </summary>
        string NameOfDictionary { get; set; }
        /// <summary>
        /// Dictionary of themes and it's paths. See the Abdrakov.Demo 
        /// </summary>
        IDictionary<T, string> ThemeSources { get; set; }
        /// <summary>
        /// Additional resources should be like: Category -> CurrentType -> Resources. See Demo project for more info
        /// </summary>
        IDictionary<string, Dictionary<string, Dictionary<string, object>>> AdditionalResources { get; set; }
        /// <summary>
        /// Current theme of an application
        /// </summary>
        T CurrentTheme { get; }

        /// <summary>
        /// Method to change current theme of an application. 
        /// Returns "false" if the theme is not registered
        /// </summary>
        /// <param name="theme">The theme that should be applied to an application</param>
        /// <returns>Is the theme applied. The theme could not be applied if it is not registered</returns>
        bool ChangeTheme(T theme);
        /// <summary>
        /// Changes additional resources in main dictionary
        /// </summary>
        /// <param name="category">The name of category</param>
        /// <param name="type">The name of requested type</param>
        /// <returns>True if rseource was found and applied</returns>
        bool ChangeAdditionalResource(string category, string type);
        /// <summary>
        /// Wrapper for the Application.TryFindResource
        /// </summary>
        /// <param name="name">The name of the resource</param>
        /// <returns>The resource</returns>
        object TryFindResource(object name);
        /// <summary>
        /// Method to get resource from selected theme
        /// </summary>
        /// <param name="name">The name of the resource</param>
        /// <param name="theme">Selected theme</param>
        /// <returns>The resource</returns>
        object TryGetResourceFromTheme(object name, T theme);
    }
}
