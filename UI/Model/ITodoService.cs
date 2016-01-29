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
        /// Gets category controller.
        /// </summary>
        ICategoryController CategoryController { get; }

        /// <summary>
        /// Gets todo controller.
        /// </summary>
        ITodoController TodoController { get; }
    }
}
