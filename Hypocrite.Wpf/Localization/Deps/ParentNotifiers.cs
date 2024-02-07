﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hypocrite.Wpf.Localization.Deps
{
    /// <summary>
    /// A memory safe dictionary storage for <see cref="ParentChangedNotifier"/> instances.
    /// </summary>
    public class ParentNotifiers
    {
        readonly Dictionary<WeakReference<DependencyObject>, ParentChangedNotifier> _inner =
            new Dictionary<WeakReference<DependencyObject>, ParentChangedNotifier>();

        /// <summary>
        /// Check, if it contains the key.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <returns>True, if the key exists.</returns>
        public bool ContainsKey(DependencyObject target)
        {
            return _inner.Keys.Any(x => x.TryGetTarget(out var item) && ReferenceEquals(item, target));
        }

        /// <summary>
        /// Removes the entry.
        /// </summary>
        /// <param name="target">The target object.</param>
        public void Remove(DependencyObject target)
        {
            if (_inner.Count == 0)
                return;

            var deadItems = new List<KeyValuePair<WeakReference<DependencyObject>, ParentChangedNotifier>>();

            foreach (var item in _inner)
            {
                // If we can't get target (== target is dead) or this is the item which we have to remove - add it to the collection for removing.
                if (!item.Key.TryGetTarget(out var itemTarget) || ReferenceEquals(itemTarget, target))
                {
                    deadItems.Add(item);
                }
            }

            foreach (var deadItem in deadItems)
            {
                deadItem.Value?.Dispose();
                _inner.Remove(deadItem.Key);
            }
        }

        /// <summary>
        /// Adds the key-value-pair.
        /// </summary>
        /// <param name="target">The target key object.</param>
        /// <param name="parentChangedNotifier">The notifier.</param>
        public void Add(DependencyObject target, ParentChangedNotifier parentChangedNotifier)
        {
            _inner.Add(new WeakReference<DependencyObject>(target), parentChangedNotifier);
        }
    }
}
