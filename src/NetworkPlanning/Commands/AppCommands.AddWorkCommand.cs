using NetworkPlanning.Extensions;
using NetworkPlanning.ViewModels;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region AddWork command

        /// <summary>
        /// Gets the AddWork command.
        /// </summary>
        public Command<EventViewModel> AddWorkCommand => _addWorkCommand ??
            (_addWorkCommand = new Command<EventViewModel>(ExecuteAddWork, CanExecuteAddWork));

        private Command<EventViewModel> _addWorkCommand;

        /// <summary>
        /// Method to invoke when the AddWork command is executed.
        /// </summary>
        private void ExecuteAddWork(EventViewModel param)
        {
            param.AddWork();
        }

        /// <summary>
        /// Method to check whether the AddWork command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteAddWork(EventViewModel param)
        {
            return true;
        }

        #endregion
    }
}
