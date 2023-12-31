using Abdrakov.CommonAvalonia.MVVM;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.MVVM.Events;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Prism.Events;
using Prism.Ioc;
using System;

namespace Abdrakov.DemoAvalonia.Views
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
            var cont = (Application.Current as AbdrakovApplication)?.Container;
            cont?.Resolve<IEventAggregator>()?.GetEvent<PreviewDoneEvent>()?.Publish();
            this.Close();
        }
    }
}
