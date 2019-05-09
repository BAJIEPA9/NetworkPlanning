using System.IO;
using System.Windows;
using NetworkPlanning.Commands;
using NetworkPlanning.Extensions;
using NetworkPlanning.Services;
using NetworkPlanning.ViewModels;
using NetworkPlanning.Views;
using Unity;

namespace NetworkPlanning
{
    public partial class App : Application
    {
        private const string SaveFileName = @"\LastSession.xml";
        private readonly IUnityContainer _container;

        public readonly string DefaultSaveFile;


        public App()
        {
            DefaultSaveFile = Directory.GetCurrentDirectory() + SaveFileName;
            var container = ServiceLocator.Container;
            RegisterTypes(container);
            _container = container;
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container
                .RegisterInstance(this)
                .RegisterType<MainViewModel>()
                .RegisterType<WorkViewModel>()
                .RegisterType<EventViewModel>()
                .RegisterType<NetworkObjectProvider>()
                .RegisterType<ViewModelProvider>()
                .RegisterType<GraphDrawingService>()
                .RegisterType<XmlService>()
                .RegisterType<AppCommands>()
                .RegisterSingleton<NetworkGraphControl>()
                .RegisterSingleton<NetworkGridControl>()
                .RegisterSingleton<EstimationControl>();

            EventExtension.Initialize(container);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (!File.Exists(DefaultSaveFile))
            {
                using (File.Create(DefaultSaveFile));
            }

            var events = _container.Resolve<NetworkObjectProvider>().Events;
            _container.Resolve<XmlService>().Export(DefaultSaveFile, events);
            base.OnExit(e);
        }
    }
}