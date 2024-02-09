using Prism.Events;

namespace Hypocrite.Events
{
    public class ThemeChangedEvent<T> : PubSubEvent<ThemeChangedEventArgs<T>>
    {
    }
}
