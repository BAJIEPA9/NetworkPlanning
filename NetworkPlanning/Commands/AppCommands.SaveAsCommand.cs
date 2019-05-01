using System.Windows.Input;
using Microsoft.Win32;

namespace NetworkPlanning.Commands
{
    internal partial class AppCommands
    {
        #region SaveAs command

        private Command<object> _saveAsCommand;

        /// <summary>
        /// Gets the SaveAs command.
        /// </summary>
        public ICommand SaveAsCommand => _saveAsCommand ??
                                    (_saveAsCommand = new Command<object>(ExecuteSaveAs, CanExecuteSaveAs));


        /// <summary>
        /// Method to invoke when the SaveAs command is executed.
        /// </summary>
        private void ExecuteSaveAs(object param)
        {
            var dialog = new SaveFileDialog
            {
                Filter = XmlFilter,
                Title = "Save graph.",
                InitialDirectory = _desktopPath
            };

            if (dialog.ShowDialog() ?? false)
            {
                _xmlService.Export(dialog.FileName, _objectProvider.Events);
            }
        }

        /// <summary>
        /// Method to check whether the SaveAs command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteSaveAs(object param)
        {
            return true;
        }

        #endregion
    }
}
