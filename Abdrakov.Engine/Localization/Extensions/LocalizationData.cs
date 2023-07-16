using System;
using System.ComponentModel;
using System.Windows;

namespace Abdrakov.Engine.Localization.Extensions
{
	public class LocalizationData : IWeakEventListener, INotifyPropertyChanged, IDisposable
	{
		public LocalizationData(string key)
		{
			_key = key;
			LanguageChangedEventManager.AddListener(this);
		}

		~LocalizationData()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				LanguageChangedEventManager.RemoveListener(this);
			}
		}

		public object Value => LocalizationManager.GetValue(_key);

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(LanguageChangedEventManager))
			{
				OnLanguageChanged(sender, e);
				return true;
			}
			return false;
		}

		private void OnLanguageChanged(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private readonly string _key;
	}
}
