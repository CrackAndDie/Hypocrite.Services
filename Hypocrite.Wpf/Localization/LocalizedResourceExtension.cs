using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Hypocrite.Localization
{
    [MarkupExtensionReturnType(typeof(object))]
    public class LocalizedResourceExtension : MarkupExtension, INotifyPropertyChanged
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

        private Binding _locBinding;

        /// <summary>
        ///  Constructor that takes no parameters
        /// </summary>
        public LocalizedResourceExtension()
        {
        }

        /// <summary>
        ///  Constructor that takes the resource key that this is a static reference to.
        /// </summary>
        public LocalizedResourceExtension(
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
            if (Key is null && _binding == null)
            {
                return null;
            }

            // if in design mode
            if (GetIsInDesignMode(serviceProvider))
            {
                if (_binding != null)
                    _key = $"Binding: {_binding.Path.Path}";
                return $"Key: {_key}";
            }

            var expr = new LocalizationChangedExpression(Key, _binding, serviceProvider);
            expr.Detaching += OnDetaching;
            _locBinding = new Binding("Value")
            {
                Source = expr,
            };

            return _locBinding.ProvideValue(serviceProvider);
        }

        internal void OnNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnDetaching()
        {
            if (_locBinding.Source is LocalizationChangedExpression expr)
            {
                expr.Detaching -= OnDetaching;
            }
            // _locBinding.Source = null; ??? 
        }

        /// <summary>
        /// Holds a SyncRoot to be thread safe
        /// </summary>
        private static readonly object SyncRoot = new object();
        /// <summary>
        /// Determines if the code is run in DesignMode or not.
        /// </summary>
        private bool? _isInDesignMode;
        /// <summary>
        /// Gets the status of the design mode
        /// </summary>
        /// <returns>TRUE if in design mode, else FALSE</returns>
        public bool GetIsInDesignMode(IServiceProvider serviceProvider)
        {
            lock (SyncRoot)
            {
                if (!(serviceProvider?.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service))
                    return false;
                DependencyObject obj = service.TargetObject as DependencyObject;
                if (obj == null) return false;

                if (_isInDesignMode.HasValue)
                    return _isInDesignMode.Value;

                if (obj.Dispatcher?.Thread == null || !obj.Dispatcher.Thread.IsAlive)
                {
                    _isInDesignMode = false;
                    return _isInDesignMode.Value;
                }

                if (!obj.Dispatcher.CheckAccess())
                {
                    try
                    {
                        _isInDesignMode = (bool)obj.Dispatcher.Invoke(DispatcherPriority.Normal, TimeSpan.FromMilliseconds(100), new Func<IServiceProvider, bool>(GetIsInDesignMode));
                    }
                    catch (Exception)
                    {
                        _isInDesignMode = default(bool);
                    }

                    return _isInDesignMode.Value;
                }
                _isInDesignMode = DesignerProperties.GetIsInDesignMode(obj);
                return _isInDesignMode.Value;
            }
        }
    }
}
