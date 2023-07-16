using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Abdrakov.Engine.Localization;
using Abdrakov.Engine.Localization.Extensions;
using Abdrakov.Engine.MVVM;
using Abdrakov.Styles.Interfaces;
using Abdrakov.Styles.Other.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System.Diagnostics;
using System.Globalization;

namespace Abdrakov.CommonWPF.Views.Other
{
    public partial class WindowHeaderView : UserControl
    {
        public string LogoImage
        {
            get { return (string)GetValue(LogoImageProperty); }
            set { SetValue(LogoImageProperty, value); }
        }

        public static readonly DependencyProperty LogoImageProperty =
            DependencyProperty.Register("LogoImage", typeof(string), typeof(WindowHeaderView));

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(WindowHeaderView));

        public Window WindowParameter
        {
            get { return (Window)GetValue(WindowParameterProperty); }
            set { SetValue(WindowParameterProperty, value); }
        }

        public static readonly DependencyProperty WindowParameterProperty =
            DependencyProperty.Register("WindowParameter", typeof(Window), typeof(WindowHeaderView));

        public ICommand MinimizeWindowCommand
        {
            get { return (ICommand)GetValue(MinimizeWindowCommandProperty); }
            set { SetValue(MinimizeWindowCommandProperty, value); }
        }

        public static readonly DependencyProperty MinimizeWindowCommandProperty =
            DependencyProperty.Register("MinimizeWindowCommand", typeof(ICommand), typeof(WindowHeaderView));

        public ICommand MaximizeWindowCommand
        {
            get { return (ICommand)GetValue(MaximizeWindowCommandProperty); }
            set { SetValue(MaximizeWindowCommandProperty, value); }
        }

        public static readonly DependencyProperty MaximizeWindowCommandProperty =
            DependencyProperty.Register("MaximizeWindowCommand", typeof(ICommand), typeof(WindowHeaderView));

        public ICommand RestoreWindowCommand
        {
            get { return (ICommand)GetValue(RestoreWindowCommandProperty); }
            set { SetValue(RestoreWindowCommandProperty, value); }
        }

        public static readonly DependencyProperty RestoreWindowCommandProperty =
            DependencyProperty.Register("RestoreWindowCommand", typeof(ICommand), typeof(WindowHeaderView));

        public ICommand CloseWindowCommand
        {
            get { return (ICommand)GetValue(CloseWindowCommandProperty); }
            set { SetValue(CloseWindowCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseWindowCommandProperty =
            DependencyProperty.Register("CloseWindowCommand", typeof(ICommand), typeof(WindowHeaderView));

        public Visibility MinimizeButtonVisibility
        {
            get { return (Visibility)GetValue(MinimizeButtonVisibilityProperty); }
            set { SetValue(MinimizeButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MinimizeButtonVisibilityProperty =
            DependencyProperty.Register("MinimizeButtonVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility MaximizeButtonVisibility
        {
            get { return (Visibility)GetValue(MaximizeButtonVisibilityProperty); }
            set { SetValue(MaximizeButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MaximizeButtonVisibilityProperty =
            DependencyProperty.Register("MaximizeButtonVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility RestoreButtonVisibility
        {
            get { return (Visibility)GetValue(RestoreButtonVisibilityProperty); }
            set { SetValue(RestoreButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RestoreButtonVisibilityProperty =
            DependencyProperty.Register("RestoreButtonVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility ProgressBarVisibility
        {
            get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
            set { SetValue(ProgressBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ProgressBarVisibilityProperty =
            DependencyProperty.Register("ProgressBarVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility CheckAllDoneVisibility
        {
            get { return (Visibility)GetValue(CheckAllDoneVisibilityProperty); }
            set { SetValue(CheckAllDoneVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CheckAllDoneVisibilityProperty =
            DependencyProperty.Register("CheckAllDoneVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility ThemeToggleVisibility
        {
            get { return (Visibility)GetValue(ThemeToggleVisibilityProperty); }
            set { SetValue(ThemeToggleVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ThemeToggleVisibilityProperty =
            DependencyProperty.Register("ThemeToggleVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public Visibility LanguagesComboBoxVisibility
        {
            get { return (Visibility)GetValue(LanguagesComboBoxVisibilityProperty); }
            set { SetValue(LanguagesComboBoxVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LanguagesComboBoxVisibilityProperty =
            DependencyProperty.Register("LanguagesComboBoxVisibility", typeof(Visibility), typeof(WindowHeaderView));

        public ObservableCollection<Language> AllowedLanguages
        {
            get { return (ObservableCollection<Language>)GetValue(AllowedLanguagesProperty); }
            set { SetValue(AllowedLanguagesProperty, value); }
        }

        public static readonly DependencyProperty AllowedLanguagesProperty =
            DependencyProperty.Register("AllowedLanguages", typeof(ObservableCollection<Language>), typeof(WindowHeaderView), new PropertyMetadata(null, AllowedLanguagesChanged));

        private static void AllowedLanguagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WindowHeaderView)d).AllowedLanguagesChanged(e.NewValue as ObservableCollection<Language>);
        }

        private void AllowedLanguagesChanged(ObservableCollection<Language> val)
        {
            if (val != null)
            {
                LanguagesComboBox.SelectedItem = val.FirstOrDefault(x => x.Name.ToLower() == LocalizationManager.CurrentLanguage.TwoLetterISOLanguageName);
            }
        }

        public WindowHeaderView()
        {
            InitializeComponent();
            var app = Application.Current as AbdrakovApplication;
            if (app.Container.IsRegistered<IAbdrakovThemeService>())
            {
                ThemeModeToggle.IsChecked = app.Container.Resolve<IAbdrakovThemeService>().IsDark;
            }
            app.Container.Resolve<IEventAggregator>().GetEvent<ThemeChangedEvent>().Subscribe((a) => 
            {
                ThemeModeToggle.IsChecked = a.IsDark;
            });
        }

        private void ThemeModeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as AbdrakovApplication;
            if (app.Container.IsRegistered<IAbdrakovThemeService>())
            {
                app.Container.Resolve<IAbdrakovThemeService>().ApplyBase((bool)ThemeModeToggle.IsChecked);
            }
        }

        private void LanguagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Language item = (Language)LanguagesComboBox.SelectedItem;
            if (!string.IsNullOrEmpty(item.Name))
            {
                LocalizationManager.CurrentLanguage = CultureInfo.GetCultureInfo(item.Name.ToLower());
            }
        }
    }
}
