using Abdrakov.CommonWPF.MVVM;
using Abdrakov.Container;
using Abdrakov.Demo.Resources.Themes;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Attributes;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Controls;
using System.Windows.Input;

namespace Abdrakov.Demo.ViewModels.HeaderViewModels
{
    public class LeftControlViewModel : ViewModelBase
    {
        [Injection]
        private IThemeSwitcherService<Themes> themeSwitcherService;

        #region Commands
        [Notify]
        public ICommand ChangeThemeCommand { get; private set; }
        #endregion

        public override void OnDependenciesReady()
        {
            base.OnDependenciesReady();
            // command registranion here
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
        }

        private void ChangeTheme()
        {
            themeSwitcherService.ChangeTheme(themeSwitcherService.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light);
            LoggingService.Info($"Current theme is {themeSwitcherService.CurrentTheme}");
        }
    }
}
