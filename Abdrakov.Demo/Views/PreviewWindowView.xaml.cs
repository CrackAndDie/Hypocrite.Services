using Abdrakov.CommonWPF.Mvvm;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Mvvm;
using Abdrakov.Engine.Mvvm.Events;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Abdrakov.Demo.Views
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
            var cont = (Application.Current as AbdrakovApplication).Container;
            cont.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Publish();
            this.Close();
        }
    }
}
