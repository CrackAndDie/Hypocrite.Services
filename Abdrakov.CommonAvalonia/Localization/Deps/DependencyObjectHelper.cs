using Avalonia;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.CommonAvalonia.Localization.Deps
{
    /// <summary>
    /// Extension methods for dependency objects.
    /// </summary>
    public static class DependencyObjectHelper
    {
        /// <summary>
        /// Gets the value thread-safe.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The value.</returns>
        public static T GetValueSync<T>(this StyledElement obj, StyledProperty<T> property)
        {
            if (obj.CheckAccess())
                return (T)obj.GetValue(property);
            else
                return (T)Dispatcher.UIThread.Invoke(new Func<object>(() => obj.GetValue(property)));
        }

        /// <summary>
        /// Sets the value thread-safe.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public static void SetValueSync<T>(this StyledElement obj, StyledProperty<T> property, T value)
        {
            if (obj.CheckAccess())
                obj.SetValue(property, value);
            else
                Dispatcher.UIThread.Invoke(new Action(() => obj.SetValue(property, value)));
        }
    }
}
