using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abdrakov.Engine.MVVM.ObserverLogics
{
    public class Observable<T> : IObservable<T>, IDisposable
    {
        private IDictionary<int, IObserver<T>> subscribers = new Dictionary<int, IObserver<T>>();
        private readonly object thisLock = new object();
        private int key;
        private bool isDisposed;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                OnCompleted();
                isDisposed = true;
            }
        }

        protected void OnNext(T value)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Observable<T>");
            }

            foreach (IObserver<T> observer in subscribers.Select(kv => kv.Value))
            {
                observer.OnNext(value);
            }
        }

        protected void OnError(Exception exception)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Observable<T>");
            }

            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            foreach (IObserver<T> observer in subscribers.Select(kv => kv.Value))
            {
                observer.OnError(exception);
            }
        }

        protected void OnCompleted()
        {
            if (isDisposed)
            {
                return;
                // throw new ObjectDisposedException("Observable<T>");
            }

            foreach (IObserver<T> observer in subscribers.Select(kv => kv.Value))
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException("observer");
            }

            lock (thisLock)
            {
                int k = key++;
                subscribers.Add(k, observer);

                OnSubscribed(observer);

                return new AnonymousDisposable(() =>
                {
                    lock (thisLock)
                    {
                        subscribers.Remove(k);
                    }
                });
            }
        }

        protected virtual void OnSubscribed(IObserver<T> observer) { }
    }

    class AnonymousDisposable : IDisposable
    {
        Action dispose;
        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose();
        }
    }
}
