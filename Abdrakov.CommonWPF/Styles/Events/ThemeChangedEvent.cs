using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.CommonWPF.Styles.Events
{
    public class ThemeChangedEvent<T> : PubSubEvent<ThemeChangedEventArgs<T>>
    {
    }
}
