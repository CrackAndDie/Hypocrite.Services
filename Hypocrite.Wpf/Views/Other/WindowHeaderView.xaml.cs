using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System.Diagnostics;
using System.Globalization;
using Prism.Regions;
using Hypocrite.Wpf.Mvvm;

namespace Hypocrite.Wpf.Views.Other
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

        public IRegionManager RegionManager 
        {
            get { return (IRegionManager)GetValue(RegionManagerProperty); }
            set { SetValue(RegionManagerProperty, value); }
        }

        public static readonly DependencyProperty RegionManagerProperty =
            DependencyProperty.Register("RegionManager", typeof(IRegionManager), typeof(WindowHeaderView));

        public WindowHeaderView()
        {
            InitializeComponent();

            // cringe but I don't want to create a vm for the shite
            RegionManager = (Application.Current as AbdrakovApplication)?.Container?.Resolve<IRegionManager>();
        }
    }
}
