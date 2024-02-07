using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Mvvm.Attributes
{
    /// <summary>
    /// Attribute that marks property for INotifyPropertyChanged weaving. Provided by Abdrakov.Fody package.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotifyAttribute : Attribute
    {
    }
}
