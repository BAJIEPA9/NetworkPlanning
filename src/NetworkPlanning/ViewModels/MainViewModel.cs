using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Data;
using NetworkPlanning.Abstraction;
using NetworkPlanning.Commands;
using NetworkPlanning.Services;
using Unity;

namespace NetworkPlanning.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly App _app;
        private readonly NetworkObjectProvider _objectProvider;
        private readonly XmlService _xmlService;

        public MainViewModel(
            IUnityContainer container,
            NetworkObjectProvider objectProvider,
            AppCommands commands,
            XmlService xmlService,
            App app) :
            base(container)
        {
            container.RegisterInstance(this);
            _objectProvider = objectProvider;
            _xmlService = xmlService;
            _app = app;
            AppCommands = commands;
            WorksCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(objectProvider.Works);
            var groupDescription = new PropertyGroupDescription(nameof(WorkViewModel.EndEvent));
            WorksCollectionView.GroupDescriptions.Add(groupDescription);

            objectProvider.Events.CollectionChanged += (sender, e) =>
            {
                var names = groupDescription.GroupNames;

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    {
                        names.Add(e.NewItems[0]);
                    } break;

                    case NotifyCollectionChangedAction.Remove:
                    {
                        var @event = (EventViewModel) e.OldItems[0];
                        names.Remove(@event);

                        for (var i = @event.EventNumber - 1; i < names.Count; i++)
                        {
                            ((EventViewModel) names[i]).EventNumber = i + 1;
                        }

                        foreach (var work in _objectProvider.Works
                            .Where(x => x.StartEvent != null && x.StartEvent.Equals(@event)))
                        {
                            work.StartEvent.OutWorks.Remove(work);
                            work.StartEvent = null;
                        }
                    } break;

                    case NotifyCollectionChangedAction.Reset:
                    {
                        names.Clear();
                    } break;
                }
            };

            objectProvider.Works.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var work = (WorkViewModel) e.OldItems[0];

                    foreach (var w in work.EndEvent.InWorks
                        .Except(new []{work})
                        .Where(x => x.StartEvent == work.StartEvent))
                    {
                        w.OnPropertyChanged(nameof(WorkViewModel.StartEvent));
                    }

                    work.StartEvent?.OutWorks.Remove(work);
                }

                OnPropertyChanged(nameof(HasNoErrors));
            };

            LoadLastSession();
        }

        public AppCommands AppCommands { get; }
        public ListCollectionView WorksCollectionView { get; }

        public bool HasNoErrors => _objectProvider.Works.All(x => x.Error == null);

        private void LoadLastSession()
        {
            var path = _app.DefaultSaveFile;

            if (File.Exists(path))
            {
                _xmlService.Import(path);
            }
            else
            {
                using (File.Create(path));
            }
        }

        #region SaveFile property: string

        public string SaveFile
        {
            get => _saveFile;
            set => SetProperty(ref _saveFile, value);
        }

        private string _saveFile;

        #endregion
    }
}