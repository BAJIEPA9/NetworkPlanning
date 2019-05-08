using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NetworkPlanning.Abstraction;
using NetworkPlanning.Commands;
using NetworkPlanning.Services;
using Unity;

namespace NetworkPlanning.ViewModels
{
    internal class WorkViewModel : ViewModelBase, IDataErrorInfo
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
            EndEvent = @event;
            LineText.Text = Duration.ToString();
        }

        public EventViewModel[] AvailableStartEvents => _objectProvider.Events
            .Where(x => x.EventNumber < EndEvent.EventNumber)
            .ToArray();

        public AppCommands AppCommands { get; }
        public EventViewModel EndEvent { get; }
        public Line Line { get; } = new Line();
        public TextBlock LineText { get; } = new TextBlock {Foreground = Brushes.Blue};

        public bool IsRecursivelyValidating { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(StartEvent):
                    {
                        Error = null;
                        var otherWorksInEvent = EndEvent.InWorks.Except(new[] {this}).ToArray();

                        if (otherWorksInEvent.Any(x => x.StartEvent == StartEvent))
                        {
                            Error = "Duplicate work!";
                        }
                        else if (StartEvent == null)
                        {
                            Error = "Chose one.";
                        }

                        if (!IsRecursivelyValidating)
                        {
                            foreach (var work in otherWorksInEvent)
                            {
                                work.IsRecursivelyValidating = true;
                                work.OnPropertyChanged(nameof(StartEvent));
                                work.IsRecursivelyValidating = false;
                            }

                            Container.Resolve<MainViewModel>().OnPropertyChanged(nameof(MainViewModel.HasNoErrors));
                        }
                    }
                        break;
                }

                return Error;
            }
        }

        public void ArrangeLineText()
        {
            if (!LineText.IsLoaded)
            {
                Line.UpdateLayout();
            }

            Canvas.SetLeft(LineText, Math.Min(Line.X1, Line.X2) +
                                     (Math.Abs(Line.X1 - Line.X2) - LineText.ActualWidth) * 0.5d);
            Canvas.SetTop(LineText, Math.Min(Line.Y1, Line.Y2) +
                                    (Math.Abs(Line.Y1 - Line.Y2) - LineText.ActualHeight) * 0.5d);
        }

        #region StartEvent property: EventViewModel

        public EventViewModel StartEvent
        {
            get => _startEvent;
            set
            {
                if (StartEvent != null && StartEvent.OutWorks.Contains(this))
                {
                    StartEvent.OutWorks.Remove(this);
                }

                SetProperty(ref _startEvent, value);
                StartEvent?.OutWorks.Add(this);
            }
        }

        private EventViewModel _startEvent;

        #endregion

        #region Duration property: int

        public int Duration
        {
            get => _duration;
            set
            {
                if (SetProperty(ref _duration, value))
                {
                    LineText.Text = value.ToString();
                }
            }
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

        #region Error property: string

        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        private string _error;

        #endregion
    }
}