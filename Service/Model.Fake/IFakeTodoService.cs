using System.Collections.Generic;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    /// <summary>
    /// Extend of <see cref="ITodoService"/> with data storage of fake data.
    /// </summary>
    public interface IFakeTodoService : ITodoService
    {
        /// <summary>
        /// List of <see cref="ICategory"/> face objects.
        /// </summary>
        IList<FakeCategory> CategoryList { get; }

        /// <summary>
        /// List of <see cref="ITodo"/> fake objects.
        /// </summary>
        IList<FakeTodo> TodoList { get; }
    }
}
