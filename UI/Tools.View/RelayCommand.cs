using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Relay command class. 
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        /// <summary>
        /// Execute delegate.
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Execute predicate.
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method to be called when the command is invoked.</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method to be called when the command is invoked.</param>
        /// <param name="canExecute">The method that determines whether the command can execute in its current state.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// If the command does not require data to be passed, this object can be set to null.
        /// </summary>
        /// <param name="parameter">Data used by the command. </param>
        /// <returns>True if this command can be executed; otherwise, false. </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// If the command does not require data to be passed, this object can be set to null.
        /// </summary>
        /// <param name="parameter">Data used by the command. </param>
        public void Execute(object parameter) => _execute(parameter);
    }
}
