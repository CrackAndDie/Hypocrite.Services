using Hypocrite.Mvvm;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Mvvm;
using Hypocrite.Core.Utils;
using Hypocrite.Core.Utils.Settings;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Hypocrite.Core.Events;

namespace Hypocrite.DemoAvalonia.ViewModels
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

        private Bitmap logoImage;
        public Bitmap LogoImage
        {
            get { return logoImage; }
            set { SetProperty(ref logoImage, value); }
        }

        public WindowIcon WindowLogoImage => new WindowIcon(logoImage);

        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { SetProperty(ref productName, value); }
        }

        private bool minimizeButtonVisibility;
        public bool MinimizeButtonVisibility
        {
            get { return minimizeButtonVisibility; }
            set { SetProperty(ref minimizeButtonVisibility, value); }
        }

        private bool maximizeButtonVisibility;
        public bool MaximizeButtonVisibility
        {
            get { return maximizeButtonVisibility; }
            set { SetProperty(ref maximizeButtonVisibility, value); }
        }

        private bool restoreButtonVisibility;
        public bool RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set { SetProperty(ref restoreButtonVisibility, value); }
        }

        private bool progressBarVisibility;
        public bool ProgressBarVisibility
        {
            get { return progressBarVisibility; }
            set { SetProperty(ref progressBarVisibility, value); }
        }

        private bool checkAllDoneVisibility;
        public bool CheckAllDoneVisibility
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

        public override void OnViewAttached()
        {
            base.OnViewAttached();

            // command registranion here
            MinimizeWindowCommand = new DelegateCommand<object>(OnMinimizeWindowCommand);
            MaximizeWindowCommand = new DelegateCommand<object>(OnMaximizeWindowCommand);
            RestoreWindowCommand = new DelegateCommand<object>(OnRestoreWindowCommand);
            CloseWindowCommand = new DelegateCommand<object>(OnCloseWindowCommand);

            if (Container.IsRegistered<BaseWindowSettings>())
            {
                _windowSettings = Container.Resolve<BaseWindowSettings>();
                if (!string.IsNullOrWhiteSpace(_windowSettings.LogoImage))
                {
                    LogoImage = new Bitmap(AssetLoader.Open(new Uri(_windowSettings.LogoImage)));
                }
                ProductName = _windowSettings.ProductName;
                SmoothAppear = _windowSettings.SmoothAppear;

                if ((Visibility)_windowSettings.WindowProgressVisibility != Visibility.Collapsed)
                {
                    EventAggregator.GetEvent<WindowProgressChangedEvent>().Subscribe(OnProgressChanged);
                }
                CheckAllDoneVisibility = (Visibility)_windowSettings.WindowProgressVisibility == Visibility.Visible;
                ProgressBarVisibility = false;

                MaximizeButtonVisibility = (Visibility)_windowSettings.MaxResButtonsVisibility == Visibility.Visible;
                RestoreButtonVisibility = false;

                MinimizeButtonVisibility = (Visibility)_windowSettings.MinimizeButtonVisibility == Visibility.Visible;
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
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            WindowOpacity = i / 100f;
                        });
                        Thread.Sleep(5);
                    }
                    Dispatcher.UIThread.Invoke(() =>
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

        private void OnPageChanged(CoreViewModelBase vm)
        {

        }

        private void OnStateChanged(WindowState state)
        {
            if ((Visibility)_windowSettings.MaxResButtonsVisibility != Visibility.Collapsed)
            {
                MaximizeButtonVisibility = state != WindowState.Maximized;
                RestoreButtonVisibility = state == WindowState.Maximized;
            }
        }

        private void OnProgressChanged(bool isDone)
        {
            if ((Visibility)_windowSettings.WindowProgressVisibility != Visibility.Collapsed)
            {
                ProgressBarVisibility = !isDone;
                CheckAllDoneVisibility = isDone;
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
