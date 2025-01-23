﻿using Hypocrite.Container;
using Hypocrite.Core.Logging.Interfaces;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Windows;

namespace Hypocrite.DemoWpf.Views
{
    public partial class DialogWindowView : Window, IDialogWindow
    {
        public DialogWindowView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                (DataContext as IDialogAware).RequestClose += (r) => OnDialogClosedEvent();
            }
        }

        private void OnDialogClosedEvent()
        {
            
        }

        public IDialogResult Result { get; set; }

        [Injection]
        public IEventAggregator EventAggregator { get; set; }
        [Injection]
        public ILoggingService LoggingService { get; set; }
    }
}
