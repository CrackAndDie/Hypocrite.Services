using Hypocrite.Mvvm;
using Hypocrite.Core.Container;
using Hypocrite.DemoWpf.Extensions;
using Hypocrite.DemoWpf.Resources.Themes;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Mvvm.Attributes;
using Prism.Commands;
using System.Windows.Input;
using Hypocrite.Localization;

namespace Hypocrite.DemoWpf.ViewModels.HeaderViewModels
{
    public class LeftControlViewModel : ViewModelBase
    {
        [Injection]
        IWindowProgressService ProgressService { get; set; }
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
            ProgressService.AddWaiter();
            DialogService.ShowMessageDialog(LocalizationManager.GetValue("MessageDialog.Title"), LocalizationManager.GetValue("MessageDialog.Description"), DialogButtons.OK);
            LoggingService.Info($"Message dialog was shown");
            ProgressService.RemoveWaiter();
        }
    }
}
