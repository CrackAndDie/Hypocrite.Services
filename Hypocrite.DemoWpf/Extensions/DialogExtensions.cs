using Prism.Services.Dialogs;
using System.Windows;
using Hypocrite.Wpf.Mvvm;
using Abdrakov.Demo.Views.DialogViews;

namespace Abdrakov.Demo.Extensions
{
    public static class DialogExtensions
    {
        public static ButtonResult ShowMessageDialog(this IDialogService dialogService, string title, string message, DialogButtons buttons)
        {
            return dialogService.ShowCustomDialog<MessageDialogView>(new { Title = title, Message = message, Buttons = buttons });
        }

        public static ButtonResult ShowCustomDialog<DialogType>(this IDialogService dialogService, object parameters = null)
            where DialogType : FrameworkElement
        {
            var result = ButtonResult.None;
            dialogService.ShowDialog(typeof(DialogType).Name, ParseParameters(parameters), r =>
            {
                if (r is DialogResult userResult)
                {
                    result = userResult.Result;
                }
            });

            return result;
        }

        private static IDialogParameters ParseParameters(object value)
        {
            if (value == null)
            {
                return null;
            }

            return new CustomDialogParameters(value);
        }
    }
}
