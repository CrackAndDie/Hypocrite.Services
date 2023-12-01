using System;
using System.Collections.Generic;
namespace Abdrakov.Engine.Localization.Extensions.Deps
{
    /// <summary>
    /// Register missing type converters here.
    /// </summary>
    public static class RegisterMissingTypeConverters
    {
        /// <summary>
        /// A flag indication if the registration was successful.
        /// </summary>
        private static bool _registered;

        /// <summary>
        /// Registers the missing type converters.
        /// </summary>
        public static void Register()
        {
            if (_registered)
                return;

            // TypeDescriptor.AddAttributes(typeof(BitmapSource), new TypeConverterAttribute(typeof(BitmapSourceTypeConverter)));

            _registered = true;
        }
    }
}
