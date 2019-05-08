using System.Linq;
using NetworkPlanning.Extensions;
using NetworkPlanning.ViewModels;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region DeleteWork command

        /// <summary>
        /// Gets the DeleteWork command.
        /// </summary>
        public Command<WorkViewModel> DeleteWorkCommand => _deleteWorkCommand ??
            (_deleteWorkCommand = new Command<WorkViewModel>(ExecuteDeleteWork, CanExecuteDeleteWork));

        private Command<WorkViewModel> _deleteWorkCommand;

        /// <summary>
        /// Method to invoke when the DeleteWork command is executed.
        /// </summary>
        private void ExecuteDeleteWork(WorkViewModel param)
        {
            var @event = _objectProvider.Events.FirstOrDefault(x => x.EventNumber == param.EndEvent.EventNumber);
            @event.RemoveWork(param);
        }

        /// <summary>
        /// Method to check whether the DeleteWork command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteDeleteWork(WorkViewModel work)
        {
            return true;
        }

        #endregion
    }
}
