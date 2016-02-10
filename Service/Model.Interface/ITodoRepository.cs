using System.Collections.Generic;
using System.Linq;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Interface for plain todo table management.
    /// </summary>
    public interface ITodoRepository : IRepository<Todo>
    {
        /// <summary>
        /// Filters todo list by title.
        /// </summary>
        /// <param name="title">Filtering title. </param>
        /// <returns>Filtered item list. </returns>
        IEnumerable<Todo> Find(string title);

        /// <summary>
        /// Filters todo list by category.
        /// </summary>
        /// <param name="category">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        IEnumerable<Todo> Find(Category category);

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        Todo FindLast();
    }
}
