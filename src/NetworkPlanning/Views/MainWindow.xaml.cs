using System.Windows;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var container = ServiceLocator.Container;
            container.RegisterInstance(this);
            DataContext = container.Resolve<MainViewModel>();
        }
    }
}
