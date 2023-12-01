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
