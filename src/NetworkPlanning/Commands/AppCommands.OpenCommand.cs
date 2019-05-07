using System.Windows.Input;
using Microsoft.Win32;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region Open command

        private Command<object> _openCommand;

        /// <summary>
        /// Gets the Open command.
        /// </summary>
        public ICommand OpenCommand => _openCommand ??
                                    (_openCommand = new Command<object>(ExecuteOpen, CanExecuteOpen));


        /// <summary>
        /// Method to invoke when the Open command is executed.
        /// </summary>
        private void ExecuteOpen(object param)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = XmlFilter,
                Title = "Open graph.",
                InitialDirectory = _desktopPath
            };

            if (dialog.ShowDialog() ?? false)
            {
                var path = dialog.FileName;
                MainViewModel.SaveFile = path;
                _xmlService.Import(path);
            }
        }

        /// <summary>
        /// Method to check whether the Open command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteOpen(object param)
        {
            return true;
        }

        #endregion
    }
}
