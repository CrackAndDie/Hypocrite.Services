using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Abdrakov.Engine.Localization.Extensions
{
	public class LocalizedResourceExtension : MarkupExtension
    {
		public LocalizedResourceExtension(string key)
		{
			Key = key;
		}

		[ConstructorArgument("key")]
		public string Key { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Key == null)
			{
				return null;
			}

			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
			{
				return Key;
			}

			var binding = new Binding("Value")
			{
				Source = new LocalizationData(Key)
			};

			return binding.ProvideValue(serviceProvider);
		}
	}
}
