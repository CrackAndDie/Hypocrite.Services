using Hypocrite.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Hypocrite.Core.Reactive
{
    internal class PropertyChangedObservable<T> : Observable<T>
    {
        private readonly BindableObject _bindable;
        private readonly PropertyInfo _property;
        private readonly bool _onChanged;
        private readonly bool _initialCall;

        public PropertyChangedObservable(BindableObject bindable, PropertyInfo property, bool initialCall, bool onChanged = true) 
        {
            _onChanged = onChanged;
            _initialCall = initialCall;

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

        protected override void OnSubscribed(IObserver<T> observer)
        {
            base.OnSubscribed(observer);
            // call once if initial call is required
            if (_initialCall)
                OnNextOuter((T)_property?.GetValue(_bindable));
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
