using Hypocrite.Core.Events;
using Hypocrite.Core.Mvvm;
using System;
using System.Reflection;

namespace Hypocrite.Core.Reactive
{
    internal class PropertySetObservable<T> : Observable<T>
    {
        private readonly BindableObject _bindable;
        private readonly PropertyInfo _property;
        private readonly bool _onSet;
        private readonly bool _initialCall;

        public PropertySetObservable(BindableObject bindable, PropertyInfo property, bool initialCall, bool onSet = true)
        {
            _onSet = onSet;
            _initialCall = initialCall;

            _bindable = bindable;
            if (_onSet)
                _bindable.PropertySet += Preparer;
            else
                _bindable.PropertySetting += Preparer;

            _property = property;
        }

        private void Preparer(object sender, EventArgs e)
        {
            string propName = "";
            if (e is PropertySetEventArgs ea)
            {
                propName = ea.PropertyName;
            }
            else if (e is PropertySettingEventArgs ea2)
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
            if (_onSet)
                _bindable.PropertySet -= Preparer;
            else
                _bindable.PropertySetting -= Preparer;

            base.Dispose(disposing);
        }
    }
}
