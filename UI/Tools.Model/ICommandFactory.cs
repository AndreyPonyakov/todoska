using System;
using System.Windows.Input;

namespace Todo.UI.Tools.Model
{
    /// <summary>
    /// Interface for cretate Command class in ViewModel layer.
    /// </summary>
    public interface ICommandFactory
    {
        ICommand CreateCommand(Action<object> execute);
        ICommand CreateCommand<T>(Action<T> execute);
        ICommand CreateCommand(Action execute);
        ICommand CreateCommand(Action<object> execute, Predicate<object> canExecute);
        ICommand CreateCommand(Action execute, Func<bool> canExecute);
    }
}
