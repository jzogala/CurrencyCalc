using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyCalc.Commands
{
    // RelayCommand is used to handle commands in MVVM architecture.
    // It allows for the binding of commands from the UI to properties in the ViewModel.
    public class RelayCommand: ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        // Event that is raised when the 'can execute' status of the command changes.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Constructor takes an action to execute and an optional function to determine if the command can execute.
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        // CanExecute determines whether the command can execute in its current state.
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        // Execute performs the action defined for this command.
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
