namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Service 
    /// </summary>
    public interface ITodoService
    {
        ICategoryController CategoryController { get; }
        ITodoController TodoController { get; }
    }
}
