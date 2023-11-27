using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Abdrakov.Engine.MVVM.ObserverLogics
{
    internal class PropertyChangedObservable<T> : Observable<T>
    {
        private PropertyChangedEventHandler _handler;
        private PropertyInfo _property;

        public PropertyChangedObservable(PropertyChangedEventHandler handler, PropertyInfo property) 
        {
            _handler = handler;
            _handler += Preparer;

            _property = property;
        }

        private void Preparer(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _property.Name)
            {
                OnPropertyChanged((T)_property.GetValue(sender));
            }
        }

        public void OnPropertyChanged(T value)
        {
            OnNext(value);
        }

        protected override void Dispose(bool disposing)
        {
            _handler -= Preparer;
            base.Dispose(disposing);
        }
    }
}
