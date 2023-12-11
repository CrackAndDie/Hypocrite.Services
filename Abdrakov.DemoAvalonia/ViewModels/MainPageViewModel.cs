using Abdrakov.CommonAvalonia.Localization;
using Abdrakov.CommonAvalonia.MVVM;
using Abdrakov.CommonAvalonia.Styles.Events;
using Abdrakov.Container;
using Abdrakov.DemoAvalonia.Resources.Themes;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM.Attributes;
using Avalonia.Media;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Abdrakov.Engine.Localization;
using System.Collections.ObjectModel;
using Abdrakov.Engine.Extensions;
using Avalonia.Controls.Shapes;

namespace Abdrakov.DemoAvalonia.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        [Notify]
        public Language SelectedLanguage { get; set; }
        [Notify]
        public IBrush BindableBrush { get; set; }

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
