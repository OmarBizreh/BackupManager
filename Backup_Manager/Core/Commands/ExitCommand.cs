using System;
using System.Windows.Input;

namespace Backup_Manager.Core.Commands
{
    internal class ExitCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            App.Current.Shutdown();
        }
    }
}