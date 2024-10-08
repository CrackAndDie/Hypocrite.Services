﻿using Avalonia;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Hypocrite.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Hypocrite.Localization
{
	public class LocalizationChangedExpression : INotifyPropertyChanged, IDisposable
	{
		private string _key;
		private Binding _binding;

		private StyledElement _bindingElement;
		private BindableObject _bindableObject;

		public event Action Detaching;

        public LocalizationChangedExpression(string key, Binding binding, IServiceProvider serviceProvider)
		{
			LocalizationManager.CurrentLanguageChanged += Preparer;

			_key = key;
			_binding = binding;

			if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service))
                throw new ArgumentException($"{nameof(serviceProvider)} should contain {nameof(IProvideValueTarget)}");

            object targetObject = service.TargetObject;
			Type typp = targetObject.GetType();
            var vm = targetObject;
            if (vm is StyledElement se)
			{
                _bindingElement = se;
            }
			else if (vm is Setter setter)
			{
				_bindingElement = null; // warning: idk what to do here
            }
			else
			{
				if (binding != null)
					throw new ArgumentException($"TargetObject of {nameof(IProvideValueTarget)} has to be {nameof(StyledElement)}");
            }
            
			if (_bindingElement != null)
				_bindingElement.DetachedFromLogicalTree += OnElementDetached;

            if (_binding != null)
			{
				var dc = _bindingElement.DataContext;
				if (dc == null || !(dc is BindableObject))
				{
					_bindingElement.DataContextChanged += DataContextChangedPreparer;
					return;
				}

				_bindableObject = dc as BindableObject;
				_bindableObject.PropertyChanged += PropertyChangedPreparer;
			}

			// first call preparer to init Value
			Preparer(null, EventArgs.Empty);
		}

		private void DataContextChangedPreparer(object sender, EventArgs e)
		{
			if (_bindableObject != null)
                _bindableObject.PropertyChanged -= PropertyChangedPreparer;

            var el = (sender as StyledElement);
			var dc = el.DataContext;
			_bindableObject = dc as BindableObject;
			_bindableObject.PropertyChanged += PropertyChangedPreparer;

			Preparer(sender, EventArgs.Empty);
		}

		private void PropertyChangedPreparer(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == _binding.Path)
			{
				Preparer(sender, e);
			}
		}

		private void Preparer(object sender, EventArgs e)
		{
			BindingPreparer();

			var currLang = LocalizationManager.CurrentLanguage;
			string translated = LocalizationManager.GetValue(_key);
			if (currLang != null && !string.IsNullOrWhiteSpace(translated))
			{
				OnNextOuter(translated);
			}
			else
			{
				OnNextOuter($"Key not found: {_key}");
			}
		}

		private void BindingPreparer()
		{
			if (_binding == null)
				return;

			var dc = _bindingElement.DataContext;
			if (dc == null || !(dc is BindableObject))
				return;

			var prop = dc.GetType().GetProperty(_binding.Path, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop == null)
				return;

			var val = prop.GetValue(dc);
			if (val == null || !(val is string))
				return;

			_key = val as string;
		}

		public void OnNextOuter(string value)
		{
			Value = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		public object Value { get; private set; }
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnElementDetached(object sender, LogicalTreeAttachmentEventArgs args)
		{
			if (_bindingElement != null)
				_bindingElement.DetachedFromLogicalTree -= OnElementDetached;
			Dispose();
            Detaching?.Invoke();
        }

		public void Dispose()
		{
			LocalizationManager.CurrentLanguageChanged -= Preparer;
			if (_bindableObject != null)
			{
				_bindableObject.PropertyChanged -= PropertyChangedPreparer;
			}
			if (_bindingElement != null)
			{
				_bindingElement.DataContextChanged -= DataContextChangedPreparer;
			}
        }
	}
}
