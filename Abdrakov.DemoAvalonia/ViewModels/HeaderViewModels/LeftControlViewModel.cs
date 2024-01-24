using Abdrakov.CommonAvalonia.Localization;
using Abdrakov.CommonAvalonia.Mvvm;
using Abdrakov.Container;
using Abdrakov.DemoAvalonia.Extensions;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Mvvm.Attributes;
using Prism.Commands;
using System.Windows.Input;

namespace Abdrakov.DemoAvalonia.ViewModels.HeaderViewModels
{
    public class LeftControlViewModel : ViewModelBase
    {
        [Injection]
        IWindowProgressService? ProgressService { get; set; }
        #region Commands
        [Notify]
        public ICommand? ShowDialogCommand { get; private set; }
        #endregion

        public override void OnViewAttached()
        {
            base.OnViewAttached();
            // command registranion here
            ShowDialogCommand = new DelegateCommand(ShowDialog);
        }

        private void ShowDialog()
        {
            ProgressService!.AddWaiter();
            DialogService.ShowMessageDialog(LocalizationManager.GetValue("MessageDialog.Title"), LocalizationManager.GetValue("MessageDialog.Description"), DialogButtons.OK, (result) =>
            {
                LoggingService.Info($"Message dialog was shown");
                ProgressService.RemoveWaiter();
            });
        }
    }
}
