using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Services
{
    public class WindowProgressService : IWindowProgressService
    {
        private readonly ObservableCollection<bool> waiterList = new ObservableCollection<bool>();
        private readonly IEventAggregator _eventAggregator;

        internal WindowProgressService(IEventAggregator ea)
        {
            _eventAggregator = ea;
            waiterList.CollectionChanged += (s, a) =>
            {
                CallStateChangeEvent(waiterList.Count == 0);
            };
        }

        public void AddWaiter()
        {
            waiterList.Add(true);
        }

        public void RemoveWaiter()
        {
            waiterList.Remove(true);
        }

        public void CallStateChangeEvent(bool isEmpty)
        {
            _eventAggregator.GetEvent<WindowProgressChangedEvent>().Publish(isEmpty);
        }
    }
}
