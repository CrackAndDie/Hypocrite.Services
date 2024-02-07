using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Mvvm.Events;
using Prism.Events;
using System.Collections.ObjectModel;

namespace Hypocrite.Core.Services
{
    public class WindowProgressService : IWindowProgressService
    {
        public bool IsDone
        {
            get
            {
                lock (_lock)
                    return _waiterList.Count == 0;
            } 
        }

        private readonly ObservableCollection<bool> _waiterList = new ObservableCollection<bool>();
        private readonly IEventAggregator _eventAggregator;

        private readonly object _lock = new object();

        public WindowProgressService(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _waiterList.CollectionChanged += (s, a) =>
            {
                lock (_lock)
                    CallStateChangeEvent(_waiterList.Count == 0);
            };
        }

        public void AddWaiter()
        {
            lock (_lock)
                _waiterList.Add(true);
        }

        public bool RemoveWaiter()
        {
            lock (_lock)
                return _waiterList.Remove(true);
        }

        private void CallStateChangeEvent(bool isEmpty)
        {
            _eventAggregator.GetEvent<WindowProgressChangedEvent>().Publish(isEmpty);
        }
    }
}
