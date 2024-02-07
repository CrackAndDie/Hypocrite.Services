using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;
            return IsEmpty(enumerable);
        }

        /// <summary>
        /// Determines whether the collection is empty <para></para>
        /// Assumes collection is non-null, will throw if it is. <see cref="IsNullOrEmpty{T}"/> for more safety.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [Pure]
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            /* If this is a list, use the Count property for efficiency. 
            * The Count property is O(1) while IEnumerable.Count() is O(N). */
            if (enumerable is ICollection<T> collection)
                return collection.Count == 0;
            if (enumerable is IReadOnlyCollection<T> readonlyCollection) // Need this because IReadOnlyCollection does not inherit from ICollection
                return readonlyCollection.Count == 0;
            return !enumerable.Any();
        }

        /// <summary>
        /// Removes all the null's from the source
        /// </summary>
        [Pure]
        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;

            return source.Where(x => x != null);
        }

        /// <summary>
        /// Replaces first entrance of condition by a new value
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="filter">Condition to find the replaceable element</param>
        /// <param name="newElement">New value</param>
        /// <returns>True if value was replaced</returns>
        [Pure]
        public static bool ReplaceFirst<T>(this IList<T> source, Func<T, bool> filter, T newElement)
        {
            for (int i = 0; i < source.Count(); ++i)
            {
                if (filter(source[i]))
                {
                    source[i] = newElement;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Takes the amount of elements from the back of the source
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="count">The amount of taking elements</param>
        /// <returns>Taken element from the last positions</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count parameter cannot be equal or less than 0</exception>
        [Pure]
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            if (count >= source.Count())
            {
                return source;
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count parameter cannot be equal or less than 0");
            }
            return source.Skip(source.Count() - count);
        }
    }
}
