using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Model.TodoControllerServiceReference;

namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Service with controllers.
    /// </summary>
    public interface ITodoService
    {
        /// <summary>
        /// Category controller.
        /// </summary>
        ICategoryController CategoryController { get; }

        /// <summary>
        /// Todo controller.
        /// </summary>
        ITodoController TodoController { get; }
    }
}
