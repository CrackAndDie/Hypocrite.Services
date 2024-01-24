using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Mvvm.Events;
using Abdrakov.Logging.Interfaces;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;

namespace Abdrakov.Engine.Mvvm
{
    public class EngineViewModelBase : BindableObject, IViewModel
    {
        public EngineViewModelBase()
        {
            viewModelName = GetType().FullName;
        }

        public virtual void OnViewAttached()
        {
            LoggingService.Debug($"OnViewAttached '{ViewModelName}'");
        }

        public virtual void OnViewReady()
        {
            LoggingService.Debug($"OnViewReady '{ViewModelName}'");
            EventAggregator.GetEvent<NavigationEvent>().Publish(this);
        }

        public TView GetView<TView>()
             where TView : class
        {
            return View as TView;
        }

        public IContainerHolder Application { get; set; }
        public IContainerProvider Container => Application.Container;
        public IEventAggregator EventAggregator => Container.Resolve<IEventAggregator>();
        public ILoggingService LoggingService => Container.Resolve<ILoggingService>();
        public IWindowProgressService WindowProgressService => Container.Resolve<IWindowProgressService>();
        public string ViewModelName => viewModelName;
        
        public object View { get; set; }

        protected readonly string viewModelName;
    }
}
