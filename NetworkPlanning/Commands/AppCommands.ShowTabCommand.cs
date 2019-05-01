using Xceed.Wpf.AvalonDock.Layout;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region ShowTab command

        /// <summary>
        /// Gets the ShowTab command.
        /// </summary>
        public Command<LayoutAnchorable> ShowTabCommand => _showTabCommand ??
                                      (_showTabCommand = new Command<LayoutAnchorable>(ExecuteShowTab, CanExecuteShowTab));

        private Command<LayoutAnchorable> _showTabCommand;

        /// <summary>
        /// Method to invoke when the ShowTab command is executed.
        /// </summary>
        private void ExecuteShowTab(LayoutAnchorable param)
        {
            param.Show();
        }

        /// <summary>
        /// Method to check whether the ShowTab command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteShowTab(LayoutAnchorable param)
        {
            return true;
        }

        #endregion
    }
}
