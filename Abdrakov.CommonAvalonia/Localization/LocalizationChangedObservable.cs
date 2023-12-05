using Abdrakov.Engine.MVVM.ObserverLogics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Abdrakov.CommonAvalonia.Localization
{
    public class LocalizationChangedObservable : Observable<object>
    {
        private string _key;

        public LocalizationChangedObservable(string key)
        {
            LocalizationManager.CurrentLanguageChanged += Preparer;

            _key = key;
        }

        private void Preparer(object sender, EventArgs e)
        {
            var currLang = LocalizationManager.CurrentLanguage;
            string translated = LocalizationManager.GetValue(_key);
            if (currLang != null && translated != null)
            {
                OnNextOuter(translated);
            }
            else
            {
                OnNextOuter(_key);
            }
        }

        public void OnNextOuter(string value)
        {
            OnNext(value);
        }

        protected override void Dispose(bool disposing)
        {
            LocalizationManager.CurrentLanguageChanged -= Preparer;

            base.Dispose(disposing);
        }
    }
}
