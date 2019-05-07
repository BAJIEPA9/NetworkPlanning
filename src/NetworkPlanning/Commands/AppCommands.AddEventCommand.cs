namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region AddEvent command

        /// <summary>
        /// Gets the AddEvent command.
        /// </summary>
        public Command AddEventCommand => _addEventCommand ??
                                   (_addEventCommand = new Command(ExecuteAddEvent, CanExecuteAddEvent));

        private Command _addEventCommand;

        /// <summary>
        /// Method to invoke when the AddEvent command is executed.
        /// </summary>
        private void ExecuteAddEvent(object param)
        {
            _objectProvider.AddEvent();
        }

        /// <summary>
        /// Method to check whether the AddEvent command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteAddEvent()
        {
            return true;
        }

        #endregion
    }
}
