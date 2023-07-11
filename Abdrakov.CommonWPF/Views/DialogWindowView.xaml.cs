using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Logging.Interfaces;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;
using Unity;

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
            EventAggregator?.GetEvent<CloseDialogEvent>().Subscribe(OnDialogClosedEvent, ThreadOption.UIThread);
        }

        private void OnDialogClosedEvent()
        {
            Close();
        }

        public IDialogResult Result { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        [Dependency]
        public ILoggingService LoggingService { get; set; }
    }
}
