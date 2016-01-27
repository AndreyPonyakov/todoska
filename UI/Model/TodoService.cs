using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Model.TodoControllerServiceReference;

namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Fake implementation of <see cref="ITodoService"/>.
    /// </summary>
    public sealed class TodoService : ITodoService
    {
        /// <summary>
        /// Category controller.
        /// </summary>
        public ICategoryController CategoryController { get; }

        /// <summary>
        /// Todo controller.
        /// </summary>
        public ITodoController TodoController { get; }

        /// <summary>
        /// Private constructor of <see cref="TodoService"/>.
        /// </summary>
        public TodoService()
        {
            CategoryController = new CategoryControllerClient();
            TodoController = new TodoControllerClient();
        }
    }
}
