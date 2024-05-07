using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypocrite.Core.Reactive
{
    public class Observable<T> : IObservable<T>, IDisposable
    {
        private readonly IDictionary<int, IObserver<T>> _subscribers = new Dictionary<int, IObserver<T>>();
        private readonly object _thisLock = new object();
        private int _key;
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
			// подавляем финализацию
			GC.SuppressFinalize(this);
		}

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                OnCompleted();
				_isDisposed = true;
            }
        }

		~Observable()
		{
			Dispose(false);
		}

		protected void OnNext(T value)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Observable<T>");
            }

            foreach (IObserver<T> observer in _subscribers.Select(kv => kv.Value))
            {
                observer.OnNext(value);
            }
        }

        protected void OnError(Exception exception)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Observable<T>");
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            foreach (IObserver<T> observer in _subscribers.Select(kv => kv.Value))
            {
                observer.OnError(exception);
            }
        }

        protected void OnCompleted()
        {
            if (_isDisposed)
            {
                return;
            }

            foreach (IObserver<T> observer in _subscribers.Select(kv => kv.Value))
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            lock (_thisLock)
            {
                int k = _key++;
				_subscribers.Add(k, observer);

                OnSubscribed(observer);

                return new AnonymousDisposable(() =>
                {
                    lock (_thisLock)
                    {
						_subscribers.Remove(k);
                    }
                });
            }
        }

        protected virtual void OnSubscribed(IObserver<T> observer) { }
    }

    class AnonymousDisposable : IDisposable
    {
        readonly Action _dispose;

        public AnonymousDisposable(Action dispose)
        {
            this._dispose = dispose;
        }

        public void Dispose()
        {
			_dispose();
        }
    }
}
