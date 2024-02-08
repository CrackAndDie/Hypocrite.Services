using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hypocrite.Styles.Events
{
    public class ThemeChangedEvent<T> : PubSubEvent<ThemeChangedEventArgs<T>>
    {
    }
}
