using NetworkPlanning.ViewModels;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region DeleteEvent command

        /// <summary>
        /// Gets the DeleteEvent command.
        /// </summary>
        public Command<EventViewModel> DeleteEventCommand => _deleteEventCommand ??
            (_deleteEventCommand = new Command<EventViewModel>(ExecuteDeleteEvent, CanExecuteDeleteEvent));

        private Command<EventViewModel> _deleteEventCommand;

        /// <summary>
        /// Method to invoke when the DeleteEvent command is executed.
        /// </summary>
        private void ExecuteDeleteEvent(EventViewModel param)
        {
            _objectProvider.RemoveEvent(param.EventNumber);
        }

        /// <summary>
        /// Method to check whether the DeleteEvent command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteDeleteEvent(EventViewModel @event)
        {
            return true;
        }

        #endregion
    }
}
