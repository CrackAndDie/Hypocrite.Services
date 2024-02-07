using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Wpf.Styles.Events
{
    public class ThemeChangedEvent<T> : PubSubEvent<ThemeChangedEventArgs<T>>
    {
    }
}
