using Hypocrite.Mvvm;
using Hypocrite.DemoAvalonia.Views.DialogViews;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Services.Dialogs;
using System;

namespace Hypocrite.DemoAvalonia.Extensions
{
    public static class DialogExtensions
    {
        public static void ShowMessageDialog(this IDialogService dialogService, string title, string message, DialogButtons buttons, Action<IDialogResult> callback = null)
        {
            dialogService.ShowCustomDialog<MessageDialogView>(new { Title = title, Message = message, Buttons = buttons }, callback);
        }

        public static void ShowCustomDialog<DialogType>(this IDialogService dialogService, object parameters = null, Action<IDialogResult> callback = null)
            where DialogType : Control
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var a = typeof(DialogType).Name;
                dialogService.ShowDialog(desktop.MainWindow, typeof(DialogType).Name, ParseParameters(parameters), r =>
                {
                    if (r is DialogResult userResult)
                    {
                        callback?.Invoke(userResult);
                    }
                });
            }
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
