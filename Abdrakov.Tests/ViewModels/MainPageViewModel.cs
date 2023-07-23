using Abdrakov.Engine.MVVM;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Abdrakov.Styles.Interfaces;
using Unity;
using Abdrakov.Logging.Interfaces;

namespace Abdrakov.Tests.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool _mode = true;

        public ICommand Test1Command { get; private set; }

        public MainPageViewModel()
        {
            Test1Command = new DelegateCommand(() =>
            {
                _mode = !_mode;
                if (Container.IsRegistered<IAbdrakovThemeService>())
                {
                    Container.Resolve<IAbdrakovThemeService>().ApplyBase(_mode);
                }
                LoggingService.Info($"Current mode is {_mode}");
            });
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
        }
    }
}
