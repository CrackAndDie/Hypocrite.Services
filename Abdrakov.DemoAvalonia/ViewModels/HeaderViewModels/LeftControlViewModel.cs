using Abdrakov.CommonAvalonia.Localization;
using Abdrakov.CommonAvalonia.MVVM;
using Abdrakov.DemoAvalonia.Extensions;
using Abdrakov.Engine.MVVM.Attributes;
using Prism.Commands;
using System.Windows.Input;

namespace Abdrakov.DemoAvalonia.ViewModels.HeaderViewModels
{
    public class LeftControlViewModel : ViewModelBase
    {
        #region Commands
        [Notify]
        public ICommand ShowDialogCommand { get; private set; }
        #endregion

        public override void OnViewAttached()
        {
            base.OnViewAttached();
            // command registranion here
            ShowDialogCommand = new DelegateCommand(ShowDialog);
        }

        private void ShowDialog()
        {
            DialogService.ShowMessageDialog(LocalizationManager.GetValue("MessageDialog.Title"), LocalizationManager.GetValue("MessageDialog.Description"), DialogButtons.OK);
            LoggingService.Info($"Message dialog was shown");
        }
    }
}
