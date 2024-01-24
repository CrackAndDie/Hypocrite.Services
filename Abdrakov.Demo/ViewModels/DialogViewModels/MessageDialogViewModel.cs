using Abdrakov.CommonWPF.Mvvm;
using Abdrakov.Engine.Mvvm.Attributes;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Abdrakov.Demo.ViewModels.DialogViewModels
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

        protected override void OnKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.Enter))
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
            else if (Keyboard.IsKeyDown(Key.Escape))
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
        public string Title { get; set; }
        [Notify]
        public string Message { get; set; }

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
