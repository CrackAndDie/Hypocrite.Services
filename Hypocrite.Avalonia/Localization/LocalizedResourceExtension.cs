using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;

namespace Hypocrite.Localization
{
    public class LocalizedResourceExtension : MarkupExtension, INotifyPropertyChanged
	{
		/// <summary>
		/// Holds the Binding to get the key
		/// </summary>
		private Binding _binding = null;

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
		public LocalizedResourceExtension()
		{
		}

		/// <summary>
		///  Constructor that takes the resource key that this is a static reference to.
		/// </summary>
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
			if (Key is null && _binding == null)
			{
				return null;
			}

			// if in design mode
			if (Design.IsDesignMode)
			{
				if (_binding != null)
					_key = $"Binding: {_binding.Path}";
				return $"Key: {_key}";
			}

			var locBinding = new Binding("Value")
			{
				Source = new LocalizationChangedExpression(Key, _binding, serviceProvider),
			};

			return locBinding;
		}

		internal void OnNotifyPropertyChanged(string property)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}
