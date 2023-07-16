using System.Globalization;

namespace Abdrakov.Engine.Localization.Extensions
{
	public interface ITranslationProvider
	{
		string Translate(object obj, CultureInfo culture = null);
	}
}
