using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Model.TodoControllerServiceReference;

namespace TodoSystem.UI.Model
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Fake implementation of <see cref="ITodoService"/>.
    /// </summary>
    public sealed class TodoService : ITodoService, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoService"/> class.
        /// </summary>
        /// <param name="address">Root address string. </param>
        public TodoService(string address)
        {
            CategoryController =
                new CategoryControllerClient(
                    new BasicHttpBinding(),
                    new EndpointAddress(
                        new Uri(new Uri(address), nameof(CategoryController))));
            TodoController = new TodoControllerClient(
                new BasicHttpBinding(),
                new EndpointAddress(
                    new Uri(new Uri(address), nameof(TodoController))));
        }

        /// <summary>
        /// Gets category controller.
        /// </summary>
        public ICategoryController CategoryController { get; }

        /// <summary>
        /// Gets todo controller.
        /// </summary>
        public ITodoController TodoController { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            (CategoryController as IDisposable)?.Dispose();
            (TodoController as IDisposable)?.Dispose();
        }
    }
}
