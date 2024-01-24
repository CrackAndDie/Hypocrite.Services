using Abdrakov.Engine.Mvvm;
using Abdrakov.Engine.Mvvm.ObserverLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Extensions
{
    public static class BindableObjectExtensions
    {
        /// <summary>
        /// RaiseAndSetIfChanged fully implements a Setter for a read-write
        /// property on a BindableObject, using CallerMemberName to raise the notification
        /// and the ref to the backing field to set the property.
        /// </summary>
        /// <typeparam name="TObj">The type of the This.</typeparam>
        /// <typeparam name="TRet">The type of the return value.</typeparam>
        /// <param name="reactiveObject">The <see cref="BindableObject"/> raising the notification.</param>
        /// <param name="backingField">A Reference to the backing field for this
        /// property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property, usually
        /// automatically provided through the CallerMemberName attribute.</param>
        /// <returns>The newly set value, normally discarded.</returns>
        public static TRet RaiseAndSetIfChanged<TObj, TRet>(
            this TObj bindableObject,
            ref TRet backingField,
            TRet newValue,
            [CallerMemberName] string propertyName = null)
            where TObj : BindableObject
        {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(propertyName);
#else
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
#endif

            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return newValue;
            }
             
            bindableObject.SetProperty(ref backingField, newValue, propertyName);
            return newValue;
        }

        /// <summary>
        /// WhenPropertyChanged returns an Observable representing the
        /// property change notifications for a specific property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        /// <param name="property">The first property chain to reference. This will be a expression pointing to a end property or field.</param>
        public static IObservable<TRet> WhenPropertyChanged<TSender, TRet>(
            this TSender sender,
            Expression<Func<TSender, TRet>> property)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, pi);
            }
            return null;
        }

        /// <summary>
        /// WhenAnyPropertyChanged returns an Observable representing the
        /// property change notifications for any property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        public static IObservable<TRet> WhenAnyPropertyChanged<TSender, TRet>(
            this TSender sender)
        {
            if (sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, null);
            }
            return null;
        }

        /// <summary>
        /// WhenPropertyChanging returns an Observable representing the
        /// property changing notifications for a specific property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        /// <param name="property">The first property chain to reference. This will be a expression pointing to a end property or field.</param>
        public static IObservable<TRet> WhenPropertyChanging<TSender, TRet>(
            this TSender sender,
            Expression<Func<TSender, TRet>> property)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, pi, false);
            }
            return null;
        }

        /// <summary>
        /// WhenAnyPropertyChanging returns an Observable representing the
        /// property changing notifications for any property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        public static IObservable<TRet> WhenAnyPropertyChanging<TSender, TRet>(
            this TSender sender)
        {
            if (sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, null, false);
            }
            return null;
        }
    }
}
