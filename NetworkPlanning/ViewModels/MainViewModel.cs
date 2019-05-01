﻿using System.Collections.Specialized;
using System.IO;
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
            _xmlService = xmlService;
            _app = app;
            AppCommands = commands;
            Works = (ListCollectionView) CollectionViewSource.GetDefaultView(objectProvider.Works);
            var groupDescription = new PropertyGroupDescription(nameof(WorkViewModel.Event));
            Works.GroupDescriptions.Add(groupDescription);

            objectProvider.Events.CollectionChanged += (sender, e) =>
            {
                var names = groupDescription.GroupNames;

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    {
                        names.Add(e.NewItems[0]);
                    }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                    {
                        var @event = (EventViewModel) e.OldItems[0];
                        names.Remove(@event);

                        for (var i = @event.EventNumber - 1; i < names.Count; i++)
                        {
                            ((EventViewModel) names[i]).EventNumber = i + 1;
                        }
                    }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    {
                        names.Clear();
                    }
                        break;
                }
            };

            LoadLastSession();
        }

        #region SaveFile property: string

        public string SaveFile
        {
            get => _saveFile;
            set => SetProperty(ref _saveFile, value);
        }

        private string _saveFile;

        #endregion

        public AppCommands AppCommands { get; }
        public ListCollectionView Works { get; }

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
    }
}