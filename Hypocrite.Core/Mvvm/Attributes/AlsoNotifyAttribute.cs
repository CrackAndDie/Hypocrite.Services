using System;

namespace Hypocrite.Core.Mvvm.Attributes
{
	/// <summary>
	/// Attribute that marks property to notify other properties when current is changed. Provided by Hypocrite.Fody package.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class AlsoNotifyAttribute : Attribute
	{
		///<summary>
		/// Marks property to be notified
		///</summary>
		///<param name="property">A property that will be notified.</param>
		public AlsoNotifyAttribute(string property)
		{
		}

		///<summary>
		/// Marks properties to be notified
		///</summary>
		///<param name="property">A property that will be notified for.</param>
		///<param name="otherProperties">The properties that will be notified.</param>
		public AlsoNotifyAttribute(string property, params string[] otherProperties)
		{
		}
	}
}
