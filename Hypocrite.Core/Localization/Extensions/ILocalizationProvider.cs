using System.Collections.Generic;
using System.Globalization;

namespace Hypocrite.Core.Localization.Extensions
{
	public interface ILocalizationProvider
	{
		object GetValue(string key, CultureInfo culture = null);
		Dictionary<string, object> GetValues(CultureInfo culture = null);
	}
}
