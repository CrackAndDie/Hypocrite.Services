using System;

namespace Hypocrite.Core.Mvvm.Attributes
{
	/// <summary>
	/// Attribute that marks property for INotifyPropertyChanged weaving. Provided by Hypocrite.Fody package.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
    public sealed class NotifyAttribute : Attribute
    {
    }
}
