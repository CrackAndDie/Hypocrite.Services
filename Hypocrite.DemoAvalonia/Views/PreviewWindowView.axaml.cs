using Hypocrite.Mvvm;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Mvvm.Events;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Prism.Events;
using Prism.Ioc;
using System;

namespace Hypocrite.DemoAvalonia.Views
{
    public partial class PreviewWindowView : Window, IPreviewWindow
    {
        private DispatcherTimer timer;

        public PreviewWindowView()
        {
            InitializeComponent();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            timer.Tick += (s, a) => { CallPreviewDoneEvent(); };
            timer.Start();
        }

        public void CallPreviewDoneEvent()
        {
            timer.Stop();
            var cont = (Application.Current as ApplicationBase)?.Container;
            cont?.Resolve<IEventAggregator>()?.GetEvent<PreviewDoneEvent>()?.Publish();
            this.Close();
        }
    }
}
