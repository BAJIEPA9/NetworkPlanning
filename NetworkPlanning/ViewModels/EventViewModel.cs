using System.Collections.ObjectModel;
using NetworkPlanning.Abstraction;
using Unity;

namespace NetworkPlanning.ViewModels
{
    internal class EventViewModel : ViewModelBase
    {
        public EventViewModel(IUnityContainer container, int eventNumber) :
            base(container)
        {
            EventNumber = eventNumber;
        }

        public ObservableCollection<WorkViewModel> Works { get; } =
            new ObservableCollection<WorkViewModel>();

        #region EventNumber property: int

        public int EventNumber
        {
            get => _eventNumber;
            set => SetProperty(ref _eventNumber, value);
        }

        private int _eventNumber;

        #endregion

        #region Left property: double

        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        private double _left;

        #endregion

        #region Top property: double

        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        private double _top;

        #endregion

        public override string ToString()
        {
            return EventNumber.ToString();
        }
    }
}
