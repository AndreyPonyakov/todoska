using System;
using System.Windows.Input;

namespace Todo.UI.Tools.Model
{
    /// <summary>
    /// Interface for cretate Command class in ViewModel layer.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        ICommand CreateCommand(Action<object> execute);

        /// <summary>
        /// Create command.
        /// </summary>
        /// <typeparam name="T">Concrete type. </typeparam>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        ICommand CreateCommand<T>(Action<T> execute);

        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        ICommand CreateCommand(Action execute);

        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <param name="canExecute">Command predicate. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        ICommand CreateCommand(Action<object> execute, Predicate<object> canExecute);

        /// <summary>
        /// Create command.
        /// </summary>
        /// <param name="execute">Command action. </param>
        /// <param name="canExecute">Command predicate. </param>
        /// <returns>Instance of <see cref="ICommand"/>. </returns>
        ICommand CreateCommand(Action execute, Func<bool> canExecute);
    }
}
