using AutoMapper;

using Model.SqlCe;

using TodoSystem.Model.Implementation;
using TodoSystem.Model.SqlCe;

namespace Host
{
    /// <summary>
    /// Service for Todo Controller.
    /// </summary>
    public class TodoController : BaseTodoController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoController"/> class.
        /// </summary>
        public TodoController()
            : base(
                new SqlCeTodoRepository(
                    new TodoDbContext(),
                    new MapperConfiguration(cfg => cfg.MapTodo()).CreateMapper()))
        {
        }
    }
}
