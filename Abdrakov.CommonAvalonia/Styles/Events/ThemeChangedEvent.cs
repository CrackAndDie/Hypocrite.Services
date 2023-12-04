using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.CommonAvalonia.Styles.Events
{
    public class ThemeChangedEvent<T> : PubSubEvent<ThemeChangedEventArgs<T>>
    {
    }
}
