using System.Windows.Input;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region Save command

        private Command<object> _saveCommand;

        /// <summary>
        /// Gets the Save command.
        /// </summary>
        public ICommand SaveCommand => _saveCommand ??
                                    (_saveCommand = new Command<object>(ExecuteSave, CanExecuteSave));


        /// <summary>
        /// Method to invoke when the Save command is executed.
        /// </summary>
        private void ExecuteSave(object param)
        {
            var path = MainViewModel.SaveFile;
            _xmlService.Export(path, _objectProvider.Events);
        }

        /// <summary>
        /// Method to check whether the Save command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteSave(object param)
        {
            return true;
        }

        #endregion
    }
}
