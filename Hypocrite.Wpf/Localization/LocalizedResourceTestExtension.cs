using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Hypocrite.Localization
{
    [MarkupExtensionReturnType(typeof(object))]
    public class LocalizedResourceTestExtension : MarkupExtension, INotifyPropertyChanged
    {
        /// <summary>
        /// Holds the Binding to get the key
        /// </summary>
        private Binding _binding;
        /// <summary>
        /// Holds the Key to a .resx object
        /// </summary>
        private string _key;
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
                    OnNotifyPropertyChanged(nameof(Key));
                }
            }
        }

        /// <summary>
        /// Gets or sets the initialize value.
        /// This is ONLY used to support the localize extension in blend!
        /// </summary>
        /// <value>The initialize value.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ConstructorArgument("key")]
        public object InitializeValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///  Constructor that takes no parameters
        /// </summary>
        public LocalizedResourceTestExtension()
        {
        }

        /// <summary>
        ///  Constructor that takes the resource key that this is a static reference to.
        /// </summary>
        public LocalizedResourceTestExtension(
            object key)
        {
            if (key is Binding binding)
                _binding = binding;
            else
                Key = key?.ToString();
        }


        /// <summary>
        ///  Return an object that should be set on the targetObject's targetProperty
        ///  for this markup extension.  For DynamicResourceExtension, this is the object found in
        ///  a resource dictionary in the current parent chain that is keyed by ResourceKey
        /// </summary>
        /// <returns>
        ///  The object to set on this property.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key == null)
            {
                throw new InvalidOperationException();
            }

            return null;
        }

        internal void OnNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
