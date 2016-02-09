using System.Linq;
using System.ServiceModel;

using TodoSystem.UI.Model;
using TodoSystem.UI.Model.TodoControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel class of Todo Controller.
    /// </summary>
    public sealed class TodoControllerViewModel : BaseOrderableControllerViewModel<ITodoService, Todo, TodoViewModel>
    {
        /// <summary>
        /// ViewModel of main workspace.
        /// </summary>
        private readonly WorkspaceViewModel _workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoControllerViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand. </param>
        /// <param name="workspace">Workspace with controllers. </param>
        public TodoControllerViewModel(ICommandFactory commandFactory, WorkspaceViewModel workspace)
            : base(commandFactory)
        {
            _workspace = workspace;
        }

        /// <summary>
        /// Create new item in the todo list.
        /// </summary>
        /// <returns>A new instance of the <see cref="TodoViewModel"> class. </see>/></returns>
        public override TodoViewModel CreateItem()
        {
            var todo = new TodoViewModel(CommandFactory, Service, _workspace);
            AppendItem(todo);
            return todo;
        }

        /// <summary>
        /// Update from service.
        /// </summary>
        public override void Open()
        {
            ClearErrors(nameof(Service));
            try
            {
                Service.TodoController.SelectAll()
                    .OrderBy(t => t.Order)
                    .ToList()
                    .ForEach(item => CreateItem().Refresh(item));
            }
            catch (CommunicationException)
            {
                AppendErrors(nameof(Service), "There was no todo endpoint.");
            }
        }
    }
}
