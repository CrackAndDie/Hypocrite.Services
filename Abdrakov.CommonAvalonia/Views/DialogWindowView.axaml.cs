using Abdrakov.Container;
using Abdrakov.Engine.Mvvm.Events;
using Abdrakov.Logging.Interfaces;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Events;
using Prism.Services.Dialogs;

namespace Abdrakov.CommonAvalonia.Views
{
    public partial class DialogWindowView : Window, IDialogWindow
    {
        public DialogWindowView()
        {
            InitializeComponent();
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Owner = desktop.MainWindow;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }

        private void Window_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // EventAggregator?.GetEvent<DialogClosedEvent>().Subscribe(OnDialogClosedEvent, ThreadOption.UIThread);
            if (DataContext != null)
            {
                (DataContext as IDialogAware).RequestClose += (r) => OnDialogClosedEvent();
            }
        }

        private void Window_KeyDown(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (DataContext != null)
            {
                var method = DataContext.GetType().GetMethod("OnKeyDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (method != null && method.GetParameters().Length > 0)
                {
                    object[] parametersArray = new object[] { e };
                    method.Invoke(DataContext, parametersArray);
                }
            }
        }

        private void OnDialogClosedEvent()
        {
            Close();
        }

        public IDialogResult Result { get; set; }

        [Injection]
        public IEventAggregator EventAggregator { get; set; }
        [Injection]
        public ILoggingService LoggingService { get; set; }
    }
}
