using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetworkPlanning.Behaviour
{
    internal class UiElementDragBehavior : Behavior<UIElement>
    {
        #region Ellipse DP: Ellipse

        public static readonly DependencyProperty EllipseProperty =
            DependencyProperty.Register(
                nameof(Ellipse),
                typeof(Ellipse),
                typeof(UiElementDragBehavior),
                new PropertyMetadata(default(Ellipse)));

        public Ellipse Ellipse
        {
            get { return (Ellipse) GetValue(EllipseProperty); }
            set { SetValue(EllipseProperty, value); }
        }

        #endregion

        private bool _isDragging;
        private Brush _initialEllipseBrush;

        protected override void OnAttached()
        {
            base.OnAttached();
            _initialEllipseBrush = Ellipse.Fill.Clone();
            
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseEnter += AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseLeftButtonUp += AssociatedObjectOnMouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove -= AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            Ellipse.Fill = Brushes.DarkSalmon;
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

        private void AssociatedObjectOnMouseEnter(object sender, MouseEventArgs e)
        {
            DragEndAction();
        }

        private void AssociatedObjectOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DragEndAction();
        }

        private void DragEndAction()
        {
            Ellipse.Fill = _initialEllipseBrush;
        }
    }
}