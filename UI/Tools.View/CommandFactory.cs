using System;
using System.Windows.Input;
using Todo.UI.Tools.Model;

namespace Todo.UI.Tools.View
{
    /// <summary>
    /// Relay command factory
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        public ICommand CreateCommand(Action<object> execute)
        {
            return new RelayCommand(execute);
        }

        /// <summary>
        /// Create command.
        /// </summary>
        /// <typeparam name="T">Concrete type. </typeparam>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        public ICommand CreateCommand<T>(Action<T> execute)
        {
            return CreateCommand(obj => execute((T)obj));
        }

        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        public ICommand CreateCommand(Action execute)
        {
            return new RelayCommand(o => execute());
        }

        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <param name="canExecute">Command predicate. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        public ICommand CreateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            return new RelayCommand(execute, canExecute);
        }


        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <param name="canExecute">Command predicate. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        public ICommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            return new RelayCommand(o => execute(), o => canExecute());
        }
    }
}
