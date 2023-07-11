using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.MVVM;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Engine.Utils;
using Abdrakov.Engine.Utils.Settings;
using Prism.Commands;
using Prism.Ioc;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Abdrakov.CommonWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        private WindowState currentWindowState;
        public WindowState CurrentWindowState
        {
            get { return currentWindowState; }
            set { SetProperty(ref currentWindowState, value); OnStateChanged(currentWindowState); }
        }

        private SolidColorBrush windowHeaderBrush;
        public SolidColorBrush WindowHeaderBrush
        {
            get { return windowHeaderBrush; }
            set { SetProperty(ref windowHeaderBrush, value); }
        }

        private SolidColorBrush windowStateBrush;
        public SolidColorBrush WindowStateBrush
        {
            get { return windowStateBrush; }
            set { SetProperty(ref windowStateBrush, value); }
        }

        private string logoImage;
        public string LogoImage
        {
            get { return logoImage; }
            set { SetProperty(ref logoImage, value); }
        }

        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { SetProperty(ref productName, value); }
        }

        private Visibility minimizeButtonVisibility;
        public Visibility MinimizeButtonVisibility
        {
            get { return minimizeButtonVisibility; }
            set { SetProperty(ref minimizeButtonVisibility, value); }
        }

        private Visibility maximizeButtonVisibility;
        public Visibility MaximizeButtonVisibility
        {
            get { return maximizeButtonVisibility; }
            set { SetProperty(ref maximizeButtonVisibility, value); }
        }

        private Visibility restoreButtonVisibility;
        public Visibility RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set { SetProperty(ref restoreButtonVisibility, value); }
        }

        private Visibility progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get { return progressBarVisibility; }
            set { SetProperty(ref progressBarVisibility, value); }
        }

        private Visibility checkAllDoneVisibility;
        public Visibility CheckAllDoneVisibility
        {
            get { return checkAllDoneVisibility; }
            set { SetProperty(ref checkAllDoneVisibility, value); }
        }

        private bool allowTranparency;
        public bool AllowTranparency
        {
            get { return allowTranparency; }
            set { SetProperty(ref allowTranparency, value); }
        }

        #region Commands
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand RestoreWindowCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        public override void OnDependenciesReady()
        {
            base.OnDependenciesReady();
            // command registranion here
            MinimizeWindowCommand = new DelegateCommand<object>(OnMinimizeWindowCommand);
            MaximizeWindowCommand = new DelegateCommand<object>(OnMaximizeWindowCommand);
            RestoreWindowCommand = new DelegateCommand<object>(OnRestoreWindowCommand);
            CloseWindowCommand = new DelegateCommand<object>(OnCloseWindowCommand);

            if (Container.IsRegistered<BaseWindowSettings>())
            {
                IWindowSettings settings = Container.Resolve<BaseWindowSettings>();
                WindowHeaderBrush = settings.WindowHeaderBrush;
                WindowStateBrush = settings.WindowStateBrush;
                LogoImage = settings.LogoImage;
                ProductName = settings.ProductName;
                AllowTranparency = settings.AllowTransparency;
                if (settings.WindowProgressVisibility != Visibility.Collapsed) 
                { 
                    EventAggregator.GetEvent<WindowProgressChangedEvent>().Subscribe(OnProgressChanged);
                }
                else
                {
                    ProgressBarVisibility = Visibility.Collapsed;
                    CheckAllDoneVisibility = Visibility.Collapsed;
                }
            }
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
            EventAggregator.GetEvent<NavigationEvent>().Subscribe(OnPageChanged);
        }

        private void OnPageChanged(ViewModelBase vm)
        {
            
        }

        private void OnStateChanged(WindowState state)
        {
            MaximizeButtonVisibility = state == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
            RestoreButtonVisibility = state == WindowState.Maximized ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnProgressChanged(bool isDone)
        {
            ProgressBarVisibility = isDone ? Visibility.Collapsed : Visibility.Visible;
            CheckAllDoneVisibility = isDone ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnMinimizeWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Minimized;
        }

        private void OnMaximizeWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Maximized;
        }

        private void OnRestoreWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Normal;
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            (paramenter as Window).Close();
        }
    }
}
