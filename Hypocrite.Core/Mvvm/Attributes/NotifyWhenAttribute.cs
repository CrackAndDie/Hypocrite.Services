using System;

namespace Hypocrite.Core.Mvvm.Attributes
{
	/// <summary>
	/// Attribute that marks property to be notified when other properties are changed. Provided by Hypocrite.Fody package.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class NotifyWhenAttribute : Attribute
	{
		///<summary>
		/// Marks property to be notified
		///</summary>
		///<param name="property">A property that will be checked for changes.</param>
		public NotifyWhenAttribute(string property)
		{
		}

		///<summary>
		/// Marks properties to be notified
		///</summary>
		///<param name="property">A property that will be checked for changes.</param>
		///<param name="otherProperties">The properties that will be checked for changes.</param>
		public NotifyWhenAttribute(string property, params string[] otherProperties)
		{
		}
	}
}
