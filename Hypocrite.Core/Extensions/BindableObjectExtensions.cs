using Hypocrite.Core.Mvvm;
using Hypocrite.Core.Reactive;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Hypocrite.Core.Extensions
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
        /// <param name="bindableObject">The <see cref="BindableObject"/> raising the notification.</param>
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
            bindableObject.SetProperty(ref backingField, newValue, propertyName);
            return newValue;
        }

        /// <summary>
        /// WhenPropertySet returns an Observable representing the
        /// property set notifications for a specific property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        /// <param name="property">The first property chain to reference. This will be a expression pointing to a end property or field.</param>
        public static IObservable<TRet> WhenPropertySet<TSender, TRet>(
            this TSender sender,
            Expression<Func<TSender, TRet>> property, bool initialCall = false)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertySetObservable<TRet>(bindableObject, pi, initialCall);
            }
            return null;
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
            Expression<Func<TSender, TRet>> property, bool initialCall = false)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, pi, initialCall);
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
            this TSender sender, bool initialCall = false)
        {
            if (sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, null, initialCall);
            }
            return null;
        }

        /// <summary>
        /// WhenPropertySet returns an Observable representing the
        /// property is setting notifications for a specific property on a
        /// BindableObject.
        /// </summary>
        /// <param name="sender">The object where the property chain starts.</param>
        /// <param name="property">The first property chain to reference. This will be a expression pointing to a end property or field.</param>
        public static IObservable<TRet> WhenPropertySetting<TSender, TRet>(
            this TSender sender,
            Expression<Func<TSender, TRet>> property, bool initialCall = false)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertySetObservable<TRet>(bindableObject, pi, initialCall, false);
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
            Expression<Func<TSender, TRet>> property, bool initialCall = false)
        {
            var mi = property.Body.GetMemberInfo();
            if (mi.MemberType == MemberTypes.Property && mi is PropertyInfo pi && sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, pi, initialCall, false);
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
            this TSender sender, bool initialCall = false)
        {
            if (sender is BindableObject bindableObject)
            {
                return new PropertyChangedObservable<TRet>(bindableObject, null, initialCall, false);
            }
            return null;
        }
    }
}
