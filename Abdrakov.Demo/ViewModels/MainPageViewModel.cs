using Abdrakov.Engine.Mvvm;
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
using Unity;
using Abdrakov.Logging.Interfaces;
using System.Collections.ObjectModel;
using Abdrakov.Engine.Localization.Extensions;
using Abdrakov.Engine.Localization;
using System.Globalization;
using Abdrakov.Demo.Resources.Themes;
using System.Windows.Media;
using System.Security.Policy;
using Abdrakov.Engine.Mvvm.Events;
using Abdrakov.Engine.Mvvm.Attributes;
using Abdrakov.CommonWPF.Mvvm;
using Abdrakov.CommonWPF.Localization;
using Abdrakov.Engine.Interfaces;
using Abdrakov.CommonWPF.Styles.Events;
using Abdrakov.Engine.Extensions;
using System.Diagnostics;
using Abdrakov.Container;

namespace Abdrakov.Demo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        [Notify]
        public Language SelectedLanguage { get; set; }
        [Notify]
        public SolidColorBrush BindableBrush { get; set; }

        [Injection]
        private IThemeSwitcherService<Themes> ThemeSwitcherService { get; set; }

        public string ChangeThemeTag => "MainPage.ChangeTheme";
        public ObservableCollection<Language> Languages => LocalizationManager.Languages;

        #region Commands
        [Notify]
        public ICommand ChangeThemeCommand { get; private set; }
        #endregion

        public override void OnViewAttached()
        {
            base.OnViewAttached();

            this.WhenPropertyChanged(x => x.SelectedLanguage).Subscribe(OnSelectedLanguageChanged);

            ChangeThemeCommand = new DelegateCommand(() =>
            {
                if (Container.IsRegistered<IThemeSwitcherService<Themes>>())
                {
                    var service = Container.Resolve<IThemeSwitcherService<Themes>>();
                    service.ChangeTheme(service.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light);
                    LoggingService.Info($"Current theme is {service.CurrentTheme}");
                }
            });

            SubscribeToThemeChange();
        }

        private void SubscribeToThemeChange()
        {
            SetTheme(ThemeSwitcherService.CurrentTheme);

            EventAggregator.GetEvent<ThemeChangedEvent<Themes>>().Subscribe((a) =>
            {
                SetTheme(a.NewTheme);
            });

            void SetTheme(Themes theme)
            {
                BindableBrush = theme == Themes.Dark ? Brushes.Blue : Brushes.BlueViolet;
            }
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
