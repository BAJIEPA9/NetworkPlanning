using System.Linq;
using NetworkPlanning.Abstraction;
using NetworkPlanning.Commands;
using NetworkPlanning.Services;
using Unity;

namespace NetworkPlanning.ViewModels
{
    internal class WorkViewModel : ViewModelBase
    {
        private readonly NetworkObjectProvider _objectProvider;

        public WorkViewModel(
            IUnityContainer container,
            NetworkObjectProvider objectProvider,
            AppCommands commands,
            EventViewModel @event) :
            base(container)
        {
            _objectProvider = objectProvider;
            AppCommands = commands;
            Event = @event;
        }

        public int[] AvailableStartEvents => _objectProvider.Events
            .Where(x => x.EventNumber < Event.EventNumber)
            .Select(y => y.EventNumber).ToArray();

        public AppCommands AppCommands { get; }
        public EventViewModel Event { get; }

        #region StartEvent property: int

        public int StartEvent
        {
            get => _startEvent;
            set => SetProperty(ref _startEvent, value);
        }

        private int _startEvent;

        #endregion

        #region Duration property: int

        public int Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private int _duration;

        #endregion

        #region Name property: string

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _name;

        #endregion
    }
}