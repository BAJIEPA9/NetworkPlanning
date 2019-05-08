using System.Linq;
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
            var node = (FrameworkElement) e.Data.GetData(e.Data.GetFormats()[0]);
            var @event = (EventViewModel) node.DataContext;
            var position = e.GetPosition(AssociatedObject);
            var halfWidth = node.ActualWidth / 2;
            var halfHeight = node.ActualHeight / 2;
            var top = position.Y - halfHeight;
            var lineTop = top + halfHeight;
            var left = position.X - halfWidth;

            @event.Top = top;
            @event.Left = left;
            Canvas.SetTop(node, top);
            Canvas.SetLeft(node, left);

            foreach (var line in @event.InWorks.Select(x => x.Line))
            {
                line.Y2 = lineTop;
                line.X2 = left;
            }

            foreach (var line in @event.OutWorks.Select(x => x.Line))
            {
                line.Y1 = lineTop;
                line.X1 = left + node.ActualWidth;
            }
        }
    }
}
