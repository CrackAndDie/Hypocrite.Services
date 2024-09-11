﻿using Avalonia;
using Avalonia.Data;
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

		public LocalizationChangedExpression(string key, Binding binding, IServiceProvider serviceProvider)
		{
			LocalizationManager.CurrentLanguageChanged += Preparer;

			_key = key;
			_binding = binding;

			if (_binding != null)
			{
				var vm = _binding.DefaultAnchor.Target;
				if (vm == null || !(vm is StyledElement))
					return;

				_bindingElement = (vm as StyledElement);
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
