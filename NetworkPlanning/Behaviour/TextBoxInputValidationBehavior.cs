using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NetworkPlanning.Behaviour
{
    internal class TextBoxInputValidationBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty RegexpProperty =
            DependencyProperty.Register(
                nameof(Regexp),
                typeof(string),
                typeof(TextBoxInputValidationBehavior),
                new FrameworkPropertyMetadata(".*"));

        public string Regexp
        {
            get => (string) GetValue(RegexpProperty);
            set => SetValue(RegexpProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectOnKeyDown;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectOnKeyDown;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        
        private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(AssociatedObject.Text + e.Text, Regexp);
        }
    }
}