using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using NetworkPlanning.Extensions;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Services
{
    internal class XmlService
    {
        private const string Postfix = "s";
        private readonly string _eventName = nameof(EventViewModel);
        private readonly string _eventsName;
        private readonly string _workName = nameof(WorkViewModel);
        private readonly string _worksName;
        private readonly NetworkObjectProvider _objectProvider;

        private readonly XmlWriterSettings _xmlSettings = new XmlWriterSettings
        {
            ConformanceLevel = ConformanceLevel.Auto,
            Indent = true
        };

        public XmlService(IUnityContainer container,
            NetworkObjectProvider objectProvider)
        {
            container.RegisterInstance(this);
            _objectProvider = objectProvider;
            _eventsName = _eventName + Postfix;
            _worksName = _workName + Postfix;
        }

        public void Export(string path, IEnumerable<EventViewModel> models)
        {
            var document = new XDocument();
            var eventsNode = new XElement(_eventsName);
            document.Add(eventsNode);

            foreach (var model in models)
            {
                var eventKeys = new List<XElement>();
                var worksNode = new XElement(_worksName);

                eventKeys.Add(new XElement(nameof(EventViewModel.EventNumber), model.EventNumber));

                foreach (var work in model.Works)
                {
                    var workNode = new XElement(_workName);
                    workNode.Add(new XElement(nameof(WorkViewModel.Name), work.Name));
                    workNode.Add(new XElement(nameof(WorkViewModel.Duration), work.Duration));
                    workNode.Add(new XElement(nameof(WorkViewModel.StartEvent), work.StartEvent));
                    worksNode.Add(workNode);
                }

                eventKeys.Add(worksNode);
                eventsNode.Add(new XElement(_eventName, eventKeys));
            }

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (var writer = XmlWriter.Create(fileStream, _xmlSettings))
                {
                    document.Save(writer);
                }
            }
        }

        public void Import(string path)
        {
            if (!File.Exists(path)) return;
            _objectProvider.Works.Clear();
            _objectProvider.Events.Clear();
            var document = XDocument.Load(path);
            var descendants = document.Descendants(_eventName);

            foreach (var descendant in descendants)
            {
                var e = _objectProvider.AddEvent();
                var worksNode = descendant.Element(_worksName);
                var workNodes = worksNode?.Elements(_workName);

                if (workNodes == null)
                {
                    MessageBox.Show($"Miss {_worksName} node.");
                    return;
                }

                foreach (var workNode in workNodes)
                {
                    var work = e.AddWork();
                    work.StartEvent = ParseInt(workNode.Element(nameof(WorkViewModel.StartEvent)));
                    work.Duration = ParseInt(workNode.Element(nameof(WorkViewModel.Duration))) ?? 0;
                    work.Name = workNode.Element(nameof(WorkViewModel.Name))?.Value;
                }
            }
        }

        private int? ParseInt(XElement element)
        {
            if (int.TryParse(element.Value, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
