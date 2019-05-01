using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NetworkPlanning.Behaviour
{
    internal class ListViewColumnBehavior : Behavior<ListView>
    {
        private GridView _gridView;

        protected override void OnAttached()
        {
            base.OnAttached();
            _gridView = AssociatedObject.View as GridView;
            AssociatedObject.SizeChanged += ListViewOnSizeChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SizeChanged -= ListViewOnSizeChanged;
        }
        
        private void ListViewOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            var columnWidth = AssociatedObject.ActualWidth / _gridView.Columns.Count;

            foreach (var column in _gridView.Columns)
            {
                column.Width = columnWidth;
            }
        }
    }
}
