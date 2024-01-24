using Abdrakov.Container;
using Abdrakov.Engine.Mvvm.Events;
using Abdrakov.Logging.Interfaces;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Windows;

namespace Abdrakov.CommonWPF.Views
{
    public partial class DialogWindowView : Window, IDialogWindow
    {
        public DialogWindowView()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EventAggregator?.GetEvent<DialogClosedEvent>().Subscribe(OnDialogClosedEvent, ThreadOption.UIThread);
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
