using System.Windows;

namespace Hypocrite.Localization
{
    public class LocalizationChangedExpression
    {
        /// <summary>
        ///     Constructor for ResourceReferenceExpression
        /// </summary>
        /// <param name="resourceKey">
        ///     Name of the resource being referenced
        /// </param>
        public LocalizationChangedExpression(object resourceKey)
        {
            _resourceKey = resourceKey;
        }

        private object _resourceKey; // Name of the resource being referenced by this expression
    }
}
