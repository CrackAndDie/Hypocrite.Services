using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Localization.Extensions.Deps
{
    /// <summary>
    /// Behavior when key is not found at the localization provider.
    /// </summary>
    public enum FallbackBehavior
    {
        /// <summary>
        /// Display "Key: {key}" string.
        /// </summary>
        Default,

        /// <summary>
        /// Display key string itself.
        /// </summary>
        Key,

        /// <summary>
        /// Display an empty string.
        /// </summary>
        EmptyString
    }
}
