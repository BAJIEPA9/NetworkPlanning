using System.Collections.ObjectModel;
using System.Linq;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Services
{
    internal class NetworkObjectProvider
    {
        private readonly ViewModelProvider _provider;

        public NetworkObjectProvider(IUnityContainer container, ViewModelProvider provider)
        {
            container.RegisterInstance(this);
            _provider = provider;
        }

        public ObservableCollection<EventViewModel> Events { get; } =
            new ObservableCollection<EventViewModel>();

        public ObservableCollection<WorkViewModel> Works { get; } =
            new ObservableCollection<WorkViewModel>();

        public EventViewModel AddEvent()
        {
            var viewModel = _provider.GetEventViewModel(Events.Count + 1);
            Events.Add(viewModel);
            return viewModel;
        }

        public void RemoveEvent(int eventNumber)
        {
            var viewModel = Events.FirstOrDefault(x => x.EventNumber == eventNumber);

            if (viewModel != null)
            {
                Events.Remove(viewModel);

                foreach (var work in viewModel.Works)
                {
                    Works.Remove(work);
                }
            }
        }
    }
}
