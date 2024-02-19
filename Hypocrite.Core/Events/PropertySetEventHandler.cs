using System;

namespace Hypocrite.Core.Events
{
    //
    // Summary:
    //     Represents the method that will handle the BindableObject.PropertySet
    //     event raised when a property is set on a component.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     A Hypocrite.Core.Events.PropertySetEventArgs that contains the event data.
    public delegate void PropertySetEventHandler(object sender, PropertySetEventArgs e);

    /// <summary>
    /// Provides data for the <see langword='PropertySet'/> event.
    /// </summary>
    public class PropertySetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Events.PropertySetEventArgs'/>
        /// class.
        /// </summary>
        public PropertySetEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Indicates the name of the property that set.
        /// </summary>
        public virtual string PropertyName { get; }
    }
}
