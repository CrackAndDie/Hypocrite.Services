using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Localization;
using Abdrakov.Engine.MVVM;
using Abdrakov.Engine.MVVM.Attributes;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Engine.Utils;
using Abdrakov.Engine.Utils.Settings;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Abdrakov.CommonWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IWindowSettings _windowSettings;

        private WindowState currentWindowState;
        public WindowState CurrentWindowState
        {
            get { return currentWindowState; }
            set { SetProperty(ref currentWindowState, value); OnStateChanged(currentWindowState); }
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

        private bool smoothAppear;
        public bool SmoothAppear
        {
            get { return smoothAppear; }
            set { SetProperty(ref smoothAppear, value); }
        }

        private float windowOpacity;
        public float WindowOpacity
        {
            get { return windowOpacity; }
            set { SetProperty(ref windowOpacity, value); }
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
                _windowSettings = Container.Resolve<BaseWindowSettings>();
                LogoImage = _windowSettings.LogoImage;
                ProductName = _windowSettings.ProductName;
                SmoothAppear = _windowSettings.SmoothAppear;

                if (_windowSettings.WindowProgressVisibility != Visibility.Collapsed) 
                { 
                    EventAggregator.GetEvent<WindowProgressChangedEvent>().Subscribe(OnProgressChanged);
                }
                CheckAllDoneVisibility = _windowSettings.WindowProgressVisibility;
                ProgressBarVisibility = Visibility.Collapsed;

                MaximizeButtonVisibility = _windowSettings.MaxResButtonsVisibility;
                RestoreButtonVisibility = Visibility.Collapsed;

                MinimizeButtonVisibility = _windowSettings.MinimizeButtonVisibility;
            }
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
            EventAggregator.GetEvent<NavigationEvent>().Subscribe(OnPageChanged);

            if (SmoothAppear)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            WindowOpacity = i / 100f;
                        });
                        Thread.Sleep(5);
                    }
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            SmoothAppear = false;
                        }
                        catch (InvalidOperationException)
                        {
                            // cannot be changed after window is shown
                        }
                    });
                });
            }
            else
            {
                WindowOpacity = 1.0f;
            }
        }

        private void OnPageChanged(ViewModelBase vm)
        {
            
        }

        private void OnStateChanged(WindowState state)
        {
            if (_windowSettings.MaxResButtonsVisibility != Visibility.Collapsed)
            {
                MaximizeButtonVisibility = state == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
                RestoreButtonVisibility = state == WindowState.Maximized ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnProgressChanged(bool isDone)
        {
            if (_windowSettings.WindowProgressVisibility != Visibility.Collapsed)
            {
                ProgressBarVisibility = isDone ? Visibility.Collapsed : Visibility.Visible;
                CheckAllDoneVisibility = isDone ? Visibility.Visible : Visibility.Collapsed;
            }
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
