using System;
using System.ServiceModel;

using CategoryClient = TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoClient = TodoSystem.UI.Model.TodoControllerServiceReference;

namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Fake implementation of <see cref="ITodoService"/>.
    /// </summary>
    /// TODO: implement dependency injection.
    public sealed class TodoService : ITodoService, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoService"/> class.
        /// </summary>
        /// <param name="address">Root address string. </param>
        public TodoService(string address)
        {
            CategoryController =
                new CategoryClient.CategoryControllerClient(
                    new BasicHttpBinding(),
                    new EndpointAddress(new Uri(new Uri(address), nameof(CategoryController))));
            TodoController =
                new TodoClient.TodoControllerClient(
                    new BasicHttpBinding(),
                    new EndpointAddress(new Uri(new Uri(address), nameof(TodoController))));

            FaultExceptionManager =
                new FaultExceptionManager("Service encountered with problems.")
                    .Register<FaultException<TodoClient.DataValidationFault>>(
                        "Operation cannot continue: content did not pass validation.")
                    .Register<FaultException<CategoryClient.DataValidationFault>>(
                        "Operation cannot continue: content did not pass validation.")
                    .Register<FaultException<TodoClient.ForeignKeyConstraintFault>>(
                        "Operation cannot continue: foreign key constraint.")
                    .Register<FaultException<CategoryClient.ForeignKeyConstraintFault>>(
                        "Operation cannot continue: foreign key constraint.")
                    .Register<FaultException<TodoClient.ItemNotFoundFault>>(
                        "Operation cannot continue: item did not find.")
                    .Register<FaultException<CategoryClient.ItemNotFoundFault>>(
                        "Operation cannot continue: item did not find.")
                    .Register<FaultException>("Operation cannot continue: unknown error.");
        }

        /// <summary>
        /// Gets category controller.
        /// </summary>
        public CategoryClient.ICategoryController CategoryController { get; }

        /// <summary>
        /// Gets todo controller.
        /// </summary>
        public TodoClient.ITodoController TodoController { get; }

        /// <summary>
        /// Gets exceptions' resolver.
        /// </summary>
        public IFaultExceptionManager FaultExceptionManager { get; }

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
