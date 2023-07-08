using System.Globalization;

namespace Shell.Engine.Localization.Extension
{
	public interface ITranslationProvider
	{
		string Translate(object obj, CultureInfo culture = null);
	}
}
