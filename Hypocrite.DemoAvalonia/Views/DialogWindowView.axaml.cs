using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Hypocrite.Core.Container;
using Hypocrite.Core.Logging.Interfaces;
using Prism.Events;
using Prism.Services.Dialogs;

namespace Hypocrite.DemoAvalonia.Views
{
    public partial class DialogWindowView : Window, IDialogWindow
    {
        public DialogWindowView()
        {
            InitializeComponent();
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
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

        public IDialogResult Result { get; set; }

        [Injection]
        public IEventAggregator EventAggregator { get; set; }
        [Injection]
        public ILoggingService LoggingService { get; set; }
    }
}
