﻿using Abdrakov.Engine.MVVM;
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
using System.Windows.Media;
using System.Security.Policy;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Styles.Events;
using Abdrakov.Engine.MVVM.Attributes;

namespace Abdrakov.Demo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Language _language;
        public Language SelectedLanguage
        {
            get { return _language; }
            set { SetProperty(ref _language, value); OnSelectedLanguageChanged(value); }
        }

        public string ChangeThemeTag => "MainPage.ChangeTheme";

        [Notify]
        public SolidColorBrush BindableBrush { get; set; }

        public ObservableCollection<Language> Languages => LocalizationManager.Languages;

        public ICommand ChangeThemeCommand { get; private set; }

        public MainPageViewModel()
        {
            ChangeThemeCommand = new DelegateCommand(() =>
            {
                if (Container.IsRegistered<ThemeSwitcherService<Themes>>())
                {
                    var service = Container.Resolve<ThemeSwitcherService<Themes>>();
                    service.ChangeTheme(service.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light);
                    LoggingService.Info($"Current theme is {service.CurrentTheme}");
                }
            });

            SubscribeToThemeChange();
        }

        private void SubscribeToThemeChange()
        {
            var service = Container.Resolve<ThemeSwitcherService<Themes>>();
            SetTheme(service.CurrentTheme);

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