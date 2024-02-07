using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Events;
using Hypocrite.Core.Logging.Interfaces;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Events;
using Prism.Services.Dialogs;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia;

namespace Hypocrite.Avalonia.Views
{
    public partial class DialogWindowView : Window, IDialogWindow
    {
        public DialogWindowView()
        {
            InitializeComponent();
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Owner = desktop.MainWindow;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // EventAggregator?.GetEvent<DialogClosedEvent>().Subscribe(OnDialogClosedEvent, ThreadOption.UIThread);
            if (DataContext != null)
            {
                (DataContext as IDialogAware).RequestClose += (r) => OnDialogClosedEvent();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
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
