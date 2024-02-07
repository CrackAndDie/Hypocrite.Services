﻿using Hypocrite.Wpf.Mvvm;
using Hypocrite.Core.Container;
using Abdrakov.Demo.Extensions;
using Abdrakov.Demo.Resources.Themes;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Mvvm.Attributes;
using Prism.Commands;
using System.Windows.Input;
using Hypocrite.Wpf.Localization;

namespace Abdrakov.Demo.ViewModels.HeaderViewModels
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
