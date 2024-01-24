using Abdrakov.CommonAvalonia.Mvvm;
using Abdrakov.Engine.Localization;
using Abdrakov.Engine.Localization.Extensions;
using Abdrakov.Logging.Interfaces;
using Avalonia;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Abdrakov.CommonAvalonia.Localization
{
    public static class LocalizationManager
    {
        public static string DefaultSeparation => ".";

        private static CultureInfo _currentLanguage = Thread.CurrentThread.CurrentUICulture;
        public static CultureInfo CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    CurrentLanguageChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private static ObservableCollection<Language> _languages;
        public static ObservableCollection<Language> Languages
        {
            get => _languages;
            private set
            {
                _languages = value;
            }
        }

        public static void AddScopedProvider(string scope, ILocalizationProvider provider)
        {
            if (!_scopedProviders.TryGetValue(scope, out var providers))
            {
                providers = new List<ILocalizationProvider>();
                _scopedProviders.Add(scope, providers);
            }

            providers.Insert(0, provider);
        }

        public static void AddTranslationProvider(Type type, ITranslationProvider provider)
        {
            if (_translationProviders.ContainsKey(type))
            {
                _translationProviders[type] = provider;
            }
            else
            {
                _translationProviders.Add(type, provider);
            }
        }

        public static List<ILocalizationProvider> GetScopedProviders(string scope)
        {
            if (_scopedProviders.TryGetValue(scope, out var provider))
            {
                return provider;
            }

            return null;
        }

        public static string GetValue(string key)
        {
            return GetValue("Gui", key, CurrentLanguage);
        }

        public static string GetValue(string scope, string key)
        {
            return GetValue(scope, key, CurrentLanguage);
        }

        public static string GetValue(string scope, string key, CultureInfo culture = null)
        {
            if (_scopedProviders.TryGetValue(scope, out var providers))
            {
                foreach (var provider in providers)
                {
                    var value = (string)provider.GetValue(key, culture);
                    if (value != null && !(value is string stringValue && string.IsNullOrEmpty(stringValue)))
                    {
                        return value;
                    }
                }

                foreach (var provider in providers)
                {
                    var value = (string)provider.GetValue(key, ResxLocalizationProvider.NeutralCulture);
                    if (value != null && !(value is string stringValue && string.IsNullOrEmpty(stringValue)))
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        public static void Initialize()
        {
            if (_scopedProviders.Count == 0)
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                InitializeExternal(currentAssembly, null);
            }
        }

        public static void InitializeExternal(Assembly assembly, ObservableCollection<Language> languages)
        {
            var assemblyName = assembly.GetName().Name;
            var providersInfo = assembly.GetManifestResourceNames()
                .Where(x => x.StartsWith($"{assemblyName}.Localization."))
                .Select(x => x.Substring($"{assemblyName}.Localization.".Length))
                .Select(x => x.Split('.'))
                .Select(x => x.TakeWhile((s, i) => i < x.Count() - 1))
                .Select(x => string.Join(".", x))
                .AsParallel()
                .Select(x => (Provider: new ResxLocalizationProvider(assembly, x), Name: x))
                .ToArray();

            foreach (var (Provider, Name) in providersInfo)
            {
                AddScopedProvider(Name, Provider);
            }

            if (languages != null)
            {
                Languages = languages;
            }
        }

        public static string Translate(object obj, CultureInfo culture = null)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (_translationProviders.TryGetValue(obj.GetType(), out var provider))
            {
                return provider.Translate(obj, culture);
            }

            foreach (var _provider in _translationProviders)
            {
                if (_provider.Key.IsAssignableFrom(obj.GetType()))
                {
                    var localizedString = _provider.Value.Translate(obj, culture);
                    if (!string.IsNullOrEmpty(localizedString))
                    {
                        return localizedString;
                    }
                }
            }

            var loggingService = (Application.Current as AbdrakovApplication)?.Container?.Resolve<ILoggingService>();
            loggingService?.Error($"No translation provider for type {obj.GetType().AssemblyQualifiedName}");

            return string.Empty;
        }

        public static event EventHandler CurrentLanguageChanged;

        private static readonly Dictionary<string, List<ILocalizationProvider>> _scopedProviders = new Dictionary<string, List<ILocalizationProvider>>();
        private static readonly Dictionary<Type, ITranslationProvider> _translationProviders = new Dictionary<Type, ITranslationProvider>();
    }
}
