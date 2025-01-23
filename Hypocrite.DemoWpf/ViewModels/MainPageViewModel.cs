using Hypocrite.Core.Mvvm;
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
using Hypocrite.Core.Logging.Interfaces;
using System.Collections.ObjectModel;
using Hypocrite.Core.Localization.Extensions;
using Hypocrite.Core.Localization;
using System.Globalization;
using Hypocrite.DemoWpf.Resources.Themes;
using System.Windows.Media;
using System.Security.Policy;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Hypocrite.Localization;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Extensions;
using System.Diagnostics;
using Hypocrite.Container;
using Hypocrite.Events;

namespace Hypocrite.DemoWpf.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        [Notify]
        public Language SelectedLanguage { get; set; } = LocalizationManager.Languages.FirstOrDefault();
        [Notify]
        [AlsoNotify(nameof(AlsoSameBindableBrush))]
        public SolidColorBrush BindableBrush { get; set; }

        [NotifyWhen(nameof(BindableBrush))]
        public SolidColorBrush SameBindableBrush => BindableBrush;

        public SolidColorBrush AlsoSameBindableBrush => BindableBrush;

		[Injection]
        private IThemeSwitcherService<ThemeType> ThemeSwitcherService { get; set; }

        public string ChangeThemeTag => "MainPage.ChangeTheme";
        public ObservableCollection<Language> Languages => LocalizationManager.Languages;

        #region Commands
        [Notify]
        public ICommand ChangeThemeCommand { get; private set; }
        #endregion

        public override void OnViewAttached()
        {
            base.OnViewAttached();

            this.WhenPropertyChanged(x => x.SelectedLanguage, true).Subscribe(OnSelectedLanguageChanged);

            ChangeThemeCommand = new DelegateCommand(() =>
            {
                if (Container.IsRegistered<IThemeSwitcherService<ThemeType>>())
                {
                    var service = Container.Resolve<IThemeSwitcherService<ThemeType>>();
                    service.ChangeTheme(service.CurrentTheme == ThemeType.Light ? ThemeType.Dark : ThemeType.Light);
                    LoggingService.Info($"Current theme is {service.CurrentTheme}");
                }
            });

            SubscribeToThemeChange();
        }

        private void SubscribeToThemeChange()
        {
            SetTheme(ThemeSwitcherService.CurrentTheme);

            EventAggregator.GetEvent<ThemeChangedEvent<ThemeType>>().Subscribe((a) =>
            {
                SetTheme(a.NewTheme);
            });

            void SetTheme(ThemeType theme)
            {
                BindableBrush = theme == ThemeType.Dark ? Brushes.Blue : Brushes.BlueViolet;
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
