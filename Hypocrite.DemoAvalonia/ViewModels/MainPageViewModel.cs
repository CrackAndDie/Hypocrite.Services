using Hypocrite.Localization;
using Hypocrite.Mvvm;
using Hypocrite.Container;
using Hypocrite.DemoAvalonia.Resources.Themes;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Mvvm.Attributes;
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
using Hypocrite.Core.Localization;
using System.Collections.ObjectModel;
using Hypocrite.Core.Extensions;
using Avalonia.Controls.Shapes;
using Hypocrite.Core.Mvvm;
using Hypocrite.Events;
using Avalonia.Controls;

namespace Hypocrite.DemoAvalonia.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        [Notify]
        public Language SelectedLanguage { get; set; } = LocalizationManager.Languages.FirstOrDefault();
        [Notify]
		[AlsoNotify(nameof(AlsoSameBindableBrush))]
		public IBrush BindableBrush { get; set; }

		[NotifyWhen(nameof(BindableBrush))]
		public IBrush SameBindableBrush => BindableBrush;

		public IBrush AlsoSameBindableBrush => BindableBrush;

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

			if (Design.IsDesignMode)
				return;

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
            SetTheme(ThemeSwitcherService!.CurrentTheme);

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
