using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM;

namespace Abdrakov.CommonWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        public override void OnDependenciesReady()
        {
            base.OnDependenciesReady();
            // command registranion here
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
            EventAggregator.GetEvent<NavigationEvent>().Subscribe(OnPageChanged);
        }

        private void OnPageChanged(ViewModelBase vm)
        {
            
        }
    }
}
