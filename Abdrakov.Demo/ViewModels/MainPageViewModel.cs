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
using System.Collections.ObjectModel;
using Abdrakov.Engine.Localization.Extensions;
using Abdrakov.Engine.Localization;
using System.Globalization;
using Abdrakov.Demo.Resources.Themes;
using Abdrakov.Styles.Services;

namespace Abdrakov.Demo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool _mode = true;

        private Language _language;
        public Language SelectedLanguage
        {
            get { return _language; }
            set { SetProperty(ref _language, value); OnSelectedLanguageChanged(value); }
        }

        public string ChangeThemeTag => "MainPage.ChangeTheme";

        public ObservableCollection<Language> Languages => LocalizationManager.Languages;

        public ICommand ChangeThemeCommand { get; private set; }

        public MainPageViewModel()
        {
            ChangeThemeCommand = new DelegateCommand(() =>
            {
                _mode = !_mode;
                if (Container.IsRegistered<ThemeSwitcherService<Themes>>())
                {
                    Container.Resolve<ThemeSwitcherService<Themes>>().ChangeTheme(_mode ? Themes.Dark : Themes.Light);
                }
                LoggingService.Info($"Current mode is {_mode}");
            });
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        private void OnSelectedLanguageChanged(Language lang)
        {
            LocalizationManager.CurrentLanguage = CultureInfo.GetCultureInfo(lang.Name.ToLower());
        }
    }
}
