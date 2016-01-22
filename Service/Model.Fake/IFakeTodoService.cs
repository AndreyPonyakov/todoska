using System.Collections.Generic;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Extend of <see cref="ITodoService"/> with data storage of fake data.
    /// </summary>
    public interface IFakeTodoService : ITodoService
    {
        /// <summary>
        /// List of <see cref="Category"/> face objects.
        /// </summary>
        IList<Category> CategoryList { get; }

        /// <summary>
        /// List of <see cref="ITodo"/> fake objects.
        /// </summary>
        IList<Todo> TodoList { get; }

    }
}
