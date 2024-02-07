﻿using Hypocrite.Core.Localization.Extensions.Deps;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Hypocrite.Wpf.Localization.Deps;
using Hypocrite.Core.Localization.Extensions;

namespace Hypocrite.Wpf.Localization
{
    [ContentProperty("ResourceIdentifierKey")]
    public class LocalizedResourceExtension : NestedMarkupExtension, INotifyPropertyChanged, IWeakEventListener, IDisposable
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
                    UpdateNewValue();

                    OnNotifyPropertyChanged(nameof(Key));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the custom value converter.
        /// </summary>
        public IValueConverter Converter
        {
            get
            {
                if (_converter == null)
                    _converter = new DefaultConverter();

                return _converter;
            }
            set => _converter = value;
        }

        /// <summary>
        /// Gets or sets the converter parameter.
        /// </summary>
        public object ConverterParameter
        {
            get => _converterParameter;
            set => _converterParameter = value;
        }

        /// <summary>
        /// Holds the Key to a .resx object
        /// </summary>
        private string _key;

        /// <summary>
        /// Holds the Binding to get the key
        /// </summary>
        private Binding _binding;

        /// <summary>
        /// the Name of the cached dynamic generated DependencyProperties
        /// </summary>
        private string cacheDPName = null;

        /// <summary>
        /// Cached DependencyProperty for this object
        /// </summary>
        private DependencyProperty cacheDPThis;

        /// <summary>
        /// Cached DependencyProperty for key string
        /// </summary>
        private DependencyProperty cacheDPKey;

        /// <summary>
        /// The last endpoint that was used for this extension.
        /// </summary>
        private SafeTargetInfo _lastEndpoint;

        /// <summary>
        /// A custom converter, supplied in the XAML code.
        /// </summary>
        private IValueConverter _converter;

        /// <summary>
        /// A parameter that can be supplied along with the converter object.
        /// </summary>
        private object _converterParameter;

        /// <summary>
        /// Behavior when key is not found at the localization provider.
        /// </summary>
        public FallbackBehavior FallbackBehavior { get; set; }

        /// <summary>
        /// Gets or sets the initialize value.
        /// This is ONLY used to support the localize extension in blend!
        /// </summary>
        /// <value>The initialize value.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ConstructorArgument("key")]
        public object InitializeValue { get; set; }

        private static readonly object ResourceBufferLock = new object();
        private static readonly object ResolveLock = new object();
        private static Dictionary<string, object> _resourceBuffer = new Dictionary<string, object>();


        internal void OnNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public LocalizedResourceExtension(object key)
		{
            if (key is TemplateBindingExpression tbe)
            {
                var newBinding = new Binding();

                var tb = tbe.TemplateBindingExtension;
                newBinding.Converter = tb.Converter;
                newBinding.ConverterParameter = tb.ConverterParameter;
                newBinding.Path = new PropertyPath(tb.Property.Name);
                newBinding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
                key = newBinding;
            }

            if (key is Binding binding)
                _binding = binding;
            else
                Key = key?.ToString();
        }

        #region TargetMarkupExtension implementation
        /// <inheritdoc/>
        public override object FormatOutput(TargetInfo endPoint, TargetInfo info)
        {
            if (_binding != null && endPoint.TargetObject is DependencyObject dpo && endPoint.TargetProperty is DependencyProperty dp)
            {
                if (GetIsInDesignMode(dpo))
                {
                    _key = $"Binding: {_binding.Path.Path}";
                }
                else
                {
                    try
                    {
                        var name = "LocalizedResource." + dp.OwnerType.FullName + "." + dp.Name;
                        if (endPoint.TargetPropertyIndex != -1)
                            name += $"[{endPoint.TargetPropertyIndex}]";

                        if (name != cacheDPName)
                        {
                            MethodInfo mi = typeof(DependencyProperty).GetMethod("FromName", BindingFlags.Static | BindingFlags.NonPublic);

                            cacheDPThis = mi.Invoke(null, new object[] { name, typeof(LocalizedResourceExtension) }) as DependencyProperty
                                ?? DependencyProperty.RegisterAttached(name, typeof(NestedMarkupExtension), typeof(LocalizedResourceExtension),
                                               new PropertyMetadata(null));

                            cacheDPKey = mi.Invoke(null, new object[] { name + ".Key", typeof(LocalizedResourceExtension) }) as DependencyProperty
                                ?? DependencyProperty.RegisterAttached(name + ".Key", typeof(string), typeof(LocalizedResourceExtension),
                                                new PropertyMetadata("", (d, e) => { (d?.GetValue(cacheDPThis) as LocalizedResourceExtension)?.UpdateNewValue(); }));
                            cacheDPName = name;
                        }

                        if (dpo.GetValue(cacheDPThis) == null)
                        {
                            BindingOperations.SetBinding(dpo, cacheDPKey, _binding);
                            dpo.SetValue(cacheDPThis, this);
                        }

                        _key = (string)dpo.GetValue(cacheDPKey);
                    }
                    catch
                    {
                        _key = _binding.Path.Path;
                    }
                }
            }

            object result = null;

            if (endPoint == null)
                return null;
            else
                _lastEndpoint = SafeTargetInfo.FromTargetInfo(endPoint);

            var targetObject = endPoint.TargetObject as DependencyObject;

            // Get target type. Change ImageSource to BitmapSource in order to use our own converter.
            var targetType = info.TargetPropertyType;

            if (targetType == typeof(ImageSource))
                targetType = typeof(BitmapSource);

            // In case of a list target, get the correct list element type.
            if ((info.TargetPropertyIndex != -1) && typeof(IList).IsAssignableFrom(info.TargetPropertyType))
                targetType = info.TargetPropertyType.GetGenericArguments()[0];

            // Try to get the localized input from the resource.
            var resourceKey = Key;
            var ci = GetForcedCultureOrDefault();

            // Extract the names of the endpoint object and property
            var epProp = GetPropertyName(endPoint.TargetProperty);
            var epName = "";

            if (endPoint.TargetObject is FrameworkElement)
                epName = ((FrameworkElement)endPoint.TargetObject).GetValueSync<string>(FrameworkElement.NameProperty);
            else if (endPoint.TargetObject is FrameworkContentElement)
                epName = ((FrameworkContentElement)endPoint.TargetObject).GetValueSync<string>(FrameworkContentElement.NameProperty);

            var resKeyBase = ci.Name + ":" + targetType.Name + ":";
            // Check, if the key is already in our resource buffer.
            object input = null;
            var isDefaultConverter = Converter is DefaultConverter;

            if (!String.IsNullOrEmpty(resourceKey))
            {
                // We've got a resource key. Try to look it up or get it from the dictionary.
                lock (ResourceBufferLock)
                {
                    if (isDefaultConverter && _resourceBuffer.ContainsKey(resKeyBase + resourceKey))
                        result = _resourceBuffer[resKeyBase + resourceKey];
                    else
                    {
                        input = LocalizationManager.GetValue(resourceKey);
                        resKeyBase += resourceKey;
                    }
                }
            }
            else
            {
                var resKeyNameProp = epName + LocalizationManager.DefaultSeparation + epProp;

                // Try the automatic lookup function.
                // First, look for a resource entry named: [FrameworkElement name][Separator][Property name]
                lock (ResourceBufferLock)
                {
                    if (isDefaultConverter && _resourceBuffer.ContainsKey(resKeyBase + resKeyNameProp))
                        result = _resourceBuffer[resKeyBase + resKeyNameProp];
                    else
                    {
                        // It was not stored in the buffer - try to retrieve it from the dictionary.
                        input = LocalizationManager.GetValue(resKeyNameProp);

                        if (input == null)
                        {
                            var resKeyName = epName;

                            // Now, try to look for a resource entry named: [FrameworkElement name]
                            // Note - this has to be nested here, as it would take precedence over the first step in the buffer lookup step.
                            if (isDefaultConverter && _resourceBuffer.ContainsKey(resKeyBase + resKeyName))
                                result = _resourceBuffer[resKeyBase + resKeyName];
                            else
                            {
                                input = LocalizationManager.GetValue(resKeyName);
                                resKeyBase += resKeyName;
                            }
                        }
                        else
                            resKeyBase += resKeyNameProp;
                    }
                }
            }

            // If no result was found, convert the input and add it to the buffer.
            if (result == null)
            {
                if (input != null)
                {
                    result = Converter.Convert(input, targetType, ConverterParameter, ci);
                    if (isDefaultConverter)
                        SafeAddItemToResourceBuffer(resKeyBase, result);
                }
                else
                {
                    if (!string.IsNullOrEmpty(_key) && (targetType == typeof(String) || targetType == typeof(object)))
                    {
                        switch (FallbackBehavior)
                        {
                            case FallbackBehavior.Key:
                                result = _key;
                                break;

                            case FallbackBehavior.EmptyString:
                                result = string.Empty;
                                break;

                            case FallbackBehavior.Default:
                            default:
                                result = "Key: " + _key;
                                break;
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool UpdateOnEndpoint(TargetInfo endpoint)
        {
            // This extension must be updated, when an endpoint is reached.
            return true;
        }
        #endregion

        /// <summary>
        /// Adds an item to the resource buffer (threadsafe).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        internal static void SafeAddItemToResourceBuffer(string key, object item)
        {
            lock (ResourceBufferLock)
            {
                if (!_resourceBuffer.ContainsKey(key))
                    _resourceBuffer.Add(key, item);
            }
        }

        /// <summary>
        /// Removes an item from the resource buffer (threadsafe).
        /// </summary>
        /// <param name="key">The key.</param>
        internal static void SafeRemoveItemFromResourceBuffer(string key)
        {
            lock (ResourceBufferLock)
            {
                if (_resourceBuffer.ContainsKey(key))
                    _resourceBuffer.Remove(key);
            }
        }

        private void ClearItemFromResourceBuffer(DictionaryEventArgs dictionaryEventArgs)
        {
            if (dictionaryEventArgs.Type == DictionaryEventType.ValueChanged && (dictionaryEventArgs.Tag is ValueChangedEventArgs vceArgs))
            {
                string ciName = (vceArgs.Tag as CultureInfo)?.Name;

                lock (ResolveLock)
                {
                    foreach (var key in _resourceBuffer.Keys.ToList())
                    {
                        if (key.EndsWith(vceArgs.Key))
                        {
                            if (ciName == null || key.StartsWith(ciName))
                            {
                                if (_resourceBuffer[key] != vceArgs.Value)
                                    SafeRemoveItemFromResourceBuffer(key);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the name of a property (regular or DependencyProperty).
        /// </summary>
        /// <param name="property">The property object.</param>
        /// <returns>The name of the property.</returns>
        private static string GetPropertyName(object property)
        {
            var epProp = "";

            if (property is PropertyInfo info)
                epProp = info.Name;
            else if (property is DependencyProperty)
            {
                epProp = ((DependencyProperty)property).Name;
            }

            // What are these names during design time good for? Any suggestions?
            if (epProp.Contains("FrameworkElementWidth5"))
                epProp = "Height";
            else if (epProp.Contains("FrameworkElementWidth6"))
                epProp = "Width";
            else if (epProp.Contains("FrameworkElementMargin12"))
                epProp = "Margin";

            return epProp;
        }

        /// <summary>
        /// If Culture property defines a valid <see cref="CultureInfo"/>, a <see cref="CultureInfo"/> instance will get
        /// created and returned, otherwise <see cref="LocalizeDictionary"/>.Culture will get returned.
        /// </summary>
        /// <returns>The <see cref="CultureInfo"/></returns>
        /// <exception cref="System.ArgumentException">
        /// thrown if the parameter Culture don't defines a valid <see cref="CultureInfo"/>
        /// </exception>
        protected CultureInfo GetForcedCultureOrDefault()
        {
            return LocalizationManager.CurrentLanguage ?? ResxLocalizationProvider.NeutralCulture;
        }

        protected override void OnFirstTargetAdded()
        {
            base.OnFirstTargetAdded();

            LanguageChangedEventManager.AddListener(this);
        }

        /// <inheritdoc />
        protected override void OnLastTargetRemoved()
        {
            base.OnLastTargetRemoved();

            LanguageChangedEventManager.RemoveListener(this);
        }

        public void ResourceChanged(DependencyObject sender, DictionaryEventArgs e)
        {
            ClearItemFromResourceBuffer(e);
            if (sender == null)
            {
                UpdateNewValue();
                return;
            }

            // Update, if this object is in our endpoint list.
            var targetDOs = (from p in GetTargetPropertyPaths()
                             select p.EndPoint.TargetObject as DependencyObject);

            foreach (var dObj in targetDOs)
            {
                var doParent = dObj;
                while (doParent != null)
                {
                    if (sender == doParent)
                    {
                        UpdateNewValue();
                        break;
                    }
                    if (!(doParent is Visual) && !(doParent is Visual3D) && !(doParent is FrameworkContentElement))
                    {
                        UpdateNewValue();
                        break;
                    }
                    try
                    {
                        DependencyObject doParent2;

                        if (doParent is FrameworkContentElement element)
                            doParent2 = element.Parent;
                        else
                            doParent2 = doParent.GetParent(true);

                        if (doParent2 == null && doParent is FrameworkElement)
                            doParent2 = ((FrameworkElement)doParent).Parent;

                        doParent = doParent2;
                    }
                    catch
                    {
                        UpdateNewValue();
                        break;
                    }
                }
            }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            ResourceChanged(null, new DictionaryEventArgs(DictionaryEventType.CultureChanged, LocalizationManager.CurrentLanguage));
            return true;
        }

        #region Helper Functions
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
        public bool GetIsInDesignMode(DependencyObject obj)
        {
            lock (SyncRoot)
            {
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
                        _isInDesignMode = (bool)obj.Dispatcher.Invoke(DispatcherPriority.Normal, TimeSpan.FromMilliseconds(100), new Func<DependencyObject, bool>(GetIsInDesignMode));
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
        #endregion
    }
}
