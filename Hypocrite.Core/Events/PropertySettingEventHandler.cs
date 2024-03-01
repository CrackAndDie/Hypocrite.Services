using System;

namespace Hypocrite.Core.Events
{
    //
    // Summary:
    //     Represents the method that will handle the BindableObject.PropertySetting
    //     event raised when a property is setting on a component.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     A Hypocrite.Core.Events.PropertySettingEventArgs that contains the event data.
    public delegate void PropertySettingEventHandler(object sender, PropertySettingEventArgs e);

    /// <summary>
    /// Provides data for the <see langword='PropertySetting'/> event.
    /// </summary>
    public class PropertySettingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Events.PropertySettingEventArgs'/>
        /// class.
        /// </summary>
        public PropertySettingEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Indicates the name of the property that is setting.
        /// </summary>
        public virtual string PropertyName { get; }
    }
}
