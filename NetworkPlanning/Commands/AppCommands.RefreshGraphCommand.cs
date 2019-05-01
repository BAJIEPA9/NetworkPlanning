using System.Windows.Controls;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region RefreshGraph command

        /// <summary>
        /// Gets the RefreshGraph command.
        /// </summary>
        public Command<Canvas> RefreshGraphCommand => _refreshGraphCommand ??
            (_refreshGraphCommand = new Command<Canvas>(ExecuteRefreshGraph, CanExecuteRefreshGraph));

        private Command<Canvas> _refreshGraphCommand;

        /// <summary>
        /// Method to invoke when the RefreshGraph command is executed.
        /// </summary>
        private void ExecuteRefreshGraph(Canvas param)
        {
            _graphService.DrawGraph(param);
        }

        /// <summary>
        /// Method to check whether the RefreshGraph command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteRefreshGraph(Canvas param)
        {
            return true;
        }

        #endregion
    }
}
