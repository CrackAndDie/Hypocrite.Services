using Abdrakov.CommonWPF.MVVM;
using Abdrakov.Container;
using Abdrakov.Demo.Extensions;
using Abdrakov.Demo.Resources.Themes;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Attributes;
using Prism.Commands;
using System.Windows.Input;
using Abdrakov.CommonWPF.Localization;

namespace Abdrakov.Demo.ViewModels.HeaderViewModels
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
