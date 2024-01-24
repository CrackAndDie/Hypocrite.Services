using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Abdrakov.Engine.Mvvm.ObserverLogics
{
    internal class PropertyChangedObservable<T> : Observable<T>
    {
        private BindableObject _bindable;
        private PropertyInfo _property;
        private bool _onChanged;

        public PropertyChangedObservable(BindableObject bindable, PropertyInfo property, bool onChanged = true) 
        {
            _onChanged = onChanged;

            _bindable = bindable;
            if (_onChanged)
                _bindable.PropertyChanged += Preparer;
            else
                _bindable.PropertyChanging += Preparer;

            _property = property;
        }

        private void Preparer(object sender, EventArgs e)
        {
            string propName = "";
            if (e is PropertyChangedEventArgs ea)
            {
                propName = ea.PropertyName;
            }
            else if (e is PropertyChangingEventArgs ea2)
            {
                propName = ea2.PropertyName;
            }

            if (_property != null)
            {
                if (propName == _property.Name)
                {
                    OnNextOuter((T)_property.GetValue(sender));
                }
            }
            // check for any property change if "_property" is null
            else
            {
                OnNextOuter((T)sender.GetType().GetRuntimeProperty(propName).GetValue(sender));
            }
        }

        public void OnNextOuter(T value)
        {
            OnNext(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (_onChanged)
                _bindable.PropertyChanged -= Preparer;
            else
                _bindable.PropertyChanging -= Preparer;

            base.Dispose(disposing);
        }
    }
}
