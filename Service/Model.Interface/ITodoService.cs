namespace Todo.Service.Model.Interface
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
