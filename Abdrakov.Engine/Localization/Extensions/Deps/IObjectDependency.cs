using System;
using System.Collections.Generic;

namespace Abdrakov.Engine.Localization.Extensions.Deps
{
    /// <summary>
    /// Interface for object which store at <see cref="ObjectDependencyManager" />.
    /// </summary>
    public interface IObjectDependency
    {
        /// <summary>
        /// Notify that some of dependencies are dead.
        /// </summary>
        void OnDependenciesRemoved(IEnumerable<WeakReference> deadDependencies);

        /// <summary>
        /// Notify that all dependencies are dead.
        /// </summary>
        void OnAllDependenciesRemoved();
    }
}
