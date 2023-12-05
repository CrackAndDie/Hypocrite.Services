using Abdrakov.CommonAvalonia.MVVM;
using Abdrakov.Container;
using Abdrakov.DemoAvalonia.Resources.Themes;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Attributes;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Abdrakov.DemoAvalonia.ViewModels.HeaderViewModels
{
    public class LeftControlViewModel : ViewModelBase
    {
        [Injection]
        private IThemeSwitcherService<Themes> ThemeSwitcherService { get; set; }

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
            ThemeSwitcherService.ChangeTheme(ThemeSwitcherService.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light);
            LoggingService.Info($"Current theme is {ThemeSwitcherService.CurrentTheme}");
        }
    }
}
