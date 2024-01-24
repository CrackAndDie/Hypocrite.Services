using Avalonia.Input;
using Abdrakov.CommonAvalonia.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;
using Abdrakov.Engine.Mvvm.Attributes;
using System.Windows.Input;

namespace Abdrakov.DemoAvalonia.ViewModels.DialogViewModels
{
    public class MessageDialogViewModel : DialogViewModelBase
    {
        public MessageDialogViewModel()
        {
            OKCommand = new DelegateCommand(() => CloseDialog(ButtonResult.OK));
            CancelCommand = new DelegateCommand(() => CloseDialog(ButtonResult.Cancel));
            YesCommand = new DelegateCommand(() => CloseDialog(ButtonResult.Yes));
            NoCommand = new DelegateCommand(() => CloseDialog(ButtonResult.No));
        }

        protected override void OnKeyDown(Avalonia.Input.KeyEventArgs keyArgs)
        {
            if (Key.Enter == keyArgs.Key)
            {
                switch (Buttons)
                {
                    case DialogButtons.OK:
                    case DialogButtons.OKCancel:
                        CloseDialog(ButtonResult.OK);
                        break;
                    case DialogButtons.YesNo:
                        CloseDialog(ButtonResult.Yes);
                        break;
                }
            }
            else if (Key.Escape == keyArgs.Key)
            {
                switch (Buttons)
                {
                    case DialogButtons.OK:
                        CloseDialog(ButtonResult.OK);
                        break;
                    case DialogButtons.OKCancel:
                        CloseDialog(ButtonResult.Cancel);
                        break;
                    case DialogButtons.YesNo:
                        CloseDialog(ButtonResult.No);
                        break;
                }
            }
        }

        [Notify]
        public DialogButtons Buttons { get; set; }
        [Notify]
        public string? Title { get; set; }
        [Notify]
        public string? Message { get; set; }

        [Notify]
        public ICommand YesCommand { get; set; }
        [Notify]
        public ICommand NoCommand { get; set; }
        [Notify]
        public ICommand OKCommand { get; set; }
        [Notify]
        public ICommand CancelCommand { get; set; }
    }
}
