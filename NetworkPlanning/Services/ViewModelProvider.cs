using NetworkPlanning.ViewModels;
using Unity;
using Unity.Resolution;

namespace NetworkPlanning.Services
{
    internal class ViewModelProvider
    {
        private readonly IUnityContainer _container;

        public ViewModelProvider(IUnityContainer container)
        {
            _container = container;
            container.RegisterInstance(this);
        }

        public EventViewModel GetEventViewModel(int eventNumber)
        {
            return _container.Resolve<EventViewModel>(
                new ParameterOverride(nameof(eventNumber), eventNumber));
        }

        public WorkViewModel GetWorkViewModel(EventViewModel @event)
        {
            return _container.Resolve<WorkViewModel>(
                new ParameterOverride(nameof(@event), @event));
        }
    }
}
