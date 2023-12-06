﻿using Abdrakov.Engine.MVVM;
using Abdrakov.Engine.MVVM.ObserverLogics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Remote.Protocol.Viewport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Abdrakov.CommonAvalonia.Localization
{
    public class LocalizationChangedObservable : Observable<object>
    {
        private string _key;
        private Binding _binding;

        private BindableObject _bindableObject;

        public LocalizationChangedObservable(string key, Binding binding)
        {
            LocalizationManager.CurrentLanguageChanged += Preparer;

            _key = key;
            _binding = binding;

            if (_binding != null)
            {
                var vm = _binding.DefaultAnchor.Target;
                if (vm == null || !(vm is StyledElement))
                    return;

                var dc = (vm as StyledElement).DataContext;
                if (dc == null || !(dc is BindableObject))
                    return;

                _bindableObject = dc as BindableObject;
                _bindableObject.PropertyChanged += PropertyChangedPreparer;
            }
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
            if (currLang != null && translated != null)
            {
                OnNextOuter(translated);
            }
            else
            {
                OnNextOuter(_key);
            }
        }

        private void BindingPreparer()
        {
            if (_binding == null)
                return;

            var vm = _binding.DefaultAnchor.Target;
            if (vm == null)
                return;

            var dc = (vm as StyledElement).DataContext;
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
            OnNext(value);
        }

        protected override void OnSubscribed(IObserver<object> observer)
        {
            base.OnSubscribed(observer);
            Preparer(null, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            LocalizationManager.CurrentLanguageChanged -= Preparer;
            if (_bindableObject != null)
            {
                _bindableObject.PropertyChanged -= PropertyChangedPreparer;
            }

            base.Dispose(disposing);
        }
    }
}
