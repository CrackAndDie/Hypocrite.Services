﻿using Hypocrite.Core.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Mvvm
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes/set.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertySetEventHandler PropertySet;
        public event PropertySettingEventHandler PropertySetting;

        // public ObservableCollection<IObservable> PropertyChangedObservables { get; set; } = new ObservableCollection<System.IObservable>();

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        public virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            return SetPropertyWithCallback(ref storage, value, null, propertyName);
        }

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <param name="onChanged">Action that is called after the property value has been changed.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        public virtual bool SetPropertyWithCallback<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            bool isDifferent = !EqualityComparer<T>.Default.Equals(storage, value);

            RaisePropertySetting(propertyName);
            if (isDifferent)
            {
                RaisePropertyChanging(propertyName);
                storage = value;
                onChanged?.Invoke();
                RaisePropertyChanged(propertyName);
            }
            RaisePropertySet(propertyName);

            return isDifferent;
        }

        /// <summary>
        /// Raises this object's PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertySetting event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertySetting([CallerMemberName] string propertyName = null)
        {
            OnPropertySetting(new PropertySettingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertySet event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertySet([CallerMemberName] string propertyName = null)
        {
            OnPropertySet(new PropertySetEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanging event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Raises this object's PropertySetting event.
        /// </summary>
        /// <param name="args">The PropertySettingEventArgs</param>
        protected virtual void OnPropertySetting(PropertySettingEventArgs args)
        {
            PropertySetting?.Invoke(this, args);
        }

        /// <summary>
        /// Raises this object's PropertySet event.
        /// </summary>
        /// <param name="args">The PropertySetEventArgs</param>
        protected virtual void OnPropertySet(PropertySetEventArgs args)
        {
            PropertySet?.Invoke(this, args);
        }
    }
}
