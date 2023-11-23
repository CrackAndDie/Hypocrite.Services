using Abdrakov.Demo.Resources.Themes;
using Abdrakov.Engine.MVVM;
using Abdrakov.Engine.MVVM.Attributes;
using Abdrakov.Styles.Services;
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
        #region Commands
        [Bindable]
        public ICommand ChangeThemeCommand { get; set; }
        #endregion

        public override void OnDependenciesReady()
        {
            base.OnDependenciesReady();
            // command registranion here
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
        }

        private void ChangeTheme()
        {
            if (Container.IsRegistered<ThemeSwitcherService<Themes>>())
            {
                var service = Container.Resolve<ThemeSwitcherService<Themes>>();
                service.ChangeTheme(service.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light);
                LoggingService.Info($"Current theme is {service.CurrentTheme}");
            }
        }
    }
}
