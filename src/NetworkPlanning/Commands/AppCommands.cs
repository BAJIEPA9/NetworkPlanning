using System;
using NetworkPlanning.Services;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        private readonly string _desktopPath;
        private const string XmlFilter = "xml (*.xml)|*.xml";
        private readonly IUnityContainer _container;
        private readonly NetworkObjectProvider _objectProvider;
        private readonly GraphDrawingService _graphService;
        private readonly XmlService _xmlService;
        private MainViewModel _mainViewModel;

        private MainViewModel MainViewModel => _mainViewModel
                                               ?? (_mainViewModel = _container.Resolve<MainViewModel>());

        public AppCommands(
            IUnityContainer container,
            NetworkObjectProvider objectProvider,
            GraphDrawingService graphService,
            XmlService xmlService)
        {
            container.RegisterInstance(this);
            _container = container;
            _objectProvider = objectProvider;
            _graphService = graphService;
            _xmlService = xmlService;
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
