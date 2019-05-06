using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using NetworkPlanning.ViewModels;

namespace NetworkPlanning.Behaviour
{
    internal class CanvasDropBehavior : Behavior<Canvas>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += AssociatedObjectOnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Drop -= AssociatedObjectOnDrop;
        }

        private void AssociatedObjectOnDrop(object sender, DragEventArgs e)
        {
            var @event = (e.Data.GetData(e.Data.GetFormats()[0]) as FrameworkElement)?.DataContext as EventViewModel;

        }
    }
}
