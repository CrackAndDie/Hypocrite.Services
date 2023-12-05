using Abdrakov.Engine.Localization.Extensions;
using Abdrakov.Engine.Localization.Extensions.Deps;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Abdrakov.CommonAvalonia.Localization
{
    public class LocalizedStaticExtension : IBinding
    {
        /// <summary>
        /// Gets or sets the Key to a .resx object
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                }
            }
        }

        /// <summary>
        /// Holds the Key to a .resx object
        /// </summary>
        private string _key;

        /// <summary>
        /// Gets or sets the initialize value.
        /// This is ONLY used to support the localize extension in blend!
        /// </summary>
        /// <value>The initialize value.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ConstructorArgument("key")]
        public object InitializeValue { get; set; }

        public LocalizedStaticExtension(object key)
        {
            Key = key?.ToString();
        }
        public IBinding ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        InstancedBinding? IBinding.Initiate(
            AvaloniaObject target,
            AvaloniaProperty? targetProperty,
            object? anchor,
            bool enableDataValidation)
        {
            if (Key is null)
            {
                return null;
            }

            var source = GetResourceObservable(Key);
            return InstancedBinding.OneWay(source);
        }

        private IObservable<object?> GetResourceObservable(string key)
        {
            return new LocalizationChangedObservable(key);
        }
    }
}
