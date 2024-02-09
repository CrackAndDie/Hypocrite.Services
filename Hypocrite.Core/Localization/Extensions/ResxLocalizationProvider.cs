using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Hypocrite.Core.Localization.Extensions
{
	public class ResxLocalizationProvider : ILocalizationProvider
	{
        public static CultureInfo NeutralCulture { get; } = CultureInfo.GetCultureInfoByIetfLanguageTag("en");

        public ResxLocalizationProvider(Assembly assembly, string file)
			 : this(new ResourceManager(assembly.GetName().Name + ".Localization." + file, assembly))
		{
		}

		public ResxLocalizationProvider(ResourceManager resourceManager)
		{
			if (resourceManager == null)
			{
				throw new ArgumentNullException(nameof(resourceManager));
			}

			var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			var resourceSets = allCultures
			.AsParallel()
			.Select(x => (Culture: x, ResourceSet: resourceManager.GetResourceSet(x, true, false)))
			.Where(x => x.ResourceSet != null)
			.ToDictionary(x => x.Culture, x => x.ResourceSet);

			foreach (var culture in allCultures)
			{
				var dictionary = new Dictionary<string, object>();
				if (resourceSets.TryGetValue(culture, out var resourceSet))
				{
					foreach (DictionaryEntry entry in resourceSet)
					{
						dictionary.Add((string)entry.Key, entry.Value);
					}
				}

				if (dictionary.Count > 0)
				{
					var languageTag = culture.IetfLanguageTag;
					if (languageTag == "")
					{
						languageTag = NeutralCulture.IetfLanguageTag;
					}

					if (!allResources.ContainsKey(languageTag))
					{
						allResources.Add(languageTag, dictionary);
					}
				}
			}

			foreach (var resourceSet in resourceSets)
			{
				resourceSet.Value.Dispose();
			}
		}

		public virtual object GetValue(string key, CultureInfo culture = null)
		{
			if (key is null)
			{
				return null;
			}

			var dictionary = GetValues(culture);
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
			}

			return null;
		}

		public Dictionary<string, object> GetValues(CultureInfo culture = null)
		{
			if (culture == null)
			{
				culture = NeutralCulture;
			}

			if (allResources.TryGetValue(culture.IetfLanguageTag, out var dictionary))
			{
				return dictionary;
			}

			return new Dictionary<string, object>();
		}

		private readonly Dictionary<string, Dictionary<string, object>> allResources = new Dictionary<string, Dictionary<string, object>>();
	}
}
