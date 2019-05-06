using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NetworkPlanning.Behaviour
{
    internal class UiElementDragBehavior : Behavior<UIElement>
    {
        private bool _isDragging;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += AssociatedObject_MouseMove;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.PreviewMouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if (_isDragging)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                    DragDrop.DoDragDrop(AssociatedObject, AssociatedObject, DragDropEffects.Move)));
                e.Handled = true;
            }
        }
    }
}