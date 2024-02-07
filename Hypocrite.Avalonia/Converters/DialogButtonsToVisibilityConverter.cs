using Hypocrite.Avalonia.Mvvm;
using Avalonia.Data.Converters;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hypocrite.Avalonia.Converters
{
    public class DialogButtonsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DialogButtons buttons && parameter is ButtonResult button)
            {
                bool show = false;
                switch (button)
                {
                    case ButtonResult.OK:
                        show = (buttons == DialogButtons.OK || buttons == DialogButtons.OKCancel);
                        break;
                    case ButtonResult.Cancel:
                        show = (buttons == DialogButtons.OKCancel);
                        break;
                    case ButtonResult.Yes:
                        show = (buttons == DialogButtons.YesNo);
                        break;
                    case ButtonResult.No:
                        show = (buttons == DialogButtons.YesNo);
                        break;
                }

                return show;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
