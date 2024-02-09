using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Hypocrite.Localization
{
    public class LanguageChangedEventManager : WeakEventManager
    {
        public static void AddListener(IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(null, listener);
        }

        public static void RemoveListener(IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(null, listener);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }

        protected override void StartListening(object source)
        {
            LocalizationManager.CurrentLanguageChanged += OnLanguageChanged;
        }

        protected override void StopListening(Object source)
        {
            LocalizationManager.CurrentLanguageChanged -= OnLanguageChanged;
        }

        private static LanguageChangedEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(LanguageChangedEventManager);
                var manager = (LanguageChangedEventManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new LanguageChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }
    }
}
