using System.Globalization;

namespace Hypocrite.Core.Localization.Extensions
{
	public interface ITranslationProvider
	{
		string Translate(object obj, CultureInfo culture = null);
	}
}
