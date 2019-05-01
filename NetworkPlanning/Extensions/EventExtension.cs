using NetworkPlanning.Services;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Extensions
{
    internal static class EventExtension
    {
        private static ViewModelProvider _provider;
        private static NetworkObjectProvider _objectProvider;


        public static void Initialize(IUnityContainer container)
        {
            _provider = container.Resolve<ViewModelProvider>();
            _objectProvider = container.Resolve<NetworkObjectProvider>();
        }

        public static WorkViewModel AddWork(this EventViewModel @event)
        {
            var viewModel = _provider.GetWorkViewModel(@event);
            @event.Works.Add(viewModel);
            _objectProvider.Works.Add(viewModel);
            return viewModel;
        }

        public static void RemoveWork(this EventViewModel @event, WorkViewModel work)
        {
            @event.Works.Remove(work);
            _objectProvider.Works.Remove(work);
        }
    }
}
