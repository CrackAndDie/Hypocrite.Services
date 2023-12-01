using Abdrakov.Engine.Localization.Extensions.Translation;
using System;
using System.Globalization;

namespace Abdrakov.CommonWPF.Localization
{
	public class ScopedTranslationProvider<T> : TranslationProviderBase<T>
	{
		public ScopedTranslationProvider(string scope, Func<T, string> tagEvaluator)
		{
			Scope = scope;
			TagEvaluator = tagEvaluator;
		}
		public override string Translate(T obj, CultureInfo culture = null)
		{
			return (string)LocalizationManager.GetValue(Scope, TagEvaluator(obj), culture);
		}

		public string Scope { get; set; }
		public Func<T, string> TagEvaluator { get; set; }
	}
}
