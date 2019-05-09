using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
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
            get => (Ellipse) GetValue(EllipseProperty);
            set => SetValue(EllipseProperty, value);
        }

        #endregion

        private bool _isDragging;
        private Style _defaultEllipseStyle;

        protected override void OnAttached()
        {
            _defaultEllipseStyle = Ellipse.Style;

            base.OnAttached();
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
            Ellipse.Style = Application.Current.FindResource("EllipseMouseDown") as Style;
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
            _isDragging = false;
            Ellipse.Style = _defaultEllipseStyle;
        }
    }
}