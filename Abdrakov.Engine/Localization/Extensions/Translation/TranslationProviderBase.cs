using System.Globalization;

namespace Shell.Engine.Localization.Extension.Translation
{
	public abstract class TranslationProviderBase<T> : ITranslationProvider
	{
		public string Translate(object obj, CultureInfo culture = null)
		{
			if (obj is T t)
			{
				return Translate(t, culture);
			}

			return string.Empty;
		}

		public abstract string Translate(T obj, CultureInfo culture = null);
	}
}
