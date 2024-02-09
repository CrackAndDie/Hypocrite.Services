using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;

namespace Hypocrite.Localization
{
    public class LocalizedResourceExtension : IBinding
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
        /// Holds the Binding to get the key
        /// </summary>
        private Binding _binding = null;

        /// <summary>
        /// Gets or sets the initialize value.
        /// This is ONLY used to support the localize extension in blend!
        /// </summary>
        /// <value>The initialize value.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ConstructorArgument("key")]
        public object InitializeValue { get; set; }

        public LocalizedResourceExtension(object key)
        {
            if (key is Binding binding)
            {
                _binding = binding;
            }
            else
            {
                Key = key?.ToString();
            }
        }
        public IBinding ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        InstancedBinding IBinding.Initiate(
            AvaloniaObject target,
            AvaloniaProperty targetProperty,
            object anchor,
            bool enableDataValidation)
        {
            if (Key is null && _binding == null)
            {
                return null;
            }

            var source = GetResourceObservable(Key, _binding);
            return InstancedBinding.OneWay(source);
        }

        private IObservable<object> GetResourceObservable(string key, Binding b = null)
        {
            return new LocalizationChangedObservable(key, b);
        }
    }
}
