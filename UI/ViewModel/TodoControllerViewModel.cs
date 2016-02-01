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
            CreateItemCommand = commandFactory.CreateCommand(
                () => CreateItem(),
                () => Service != null && workspace.GetErrors(nameof(Service)) == null);
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
        /// TODO: implement pull model of notifying errors.
        public override void Refresh()
        {
            try
            {
                List.Clear();
                Service.TodoController.SelectAll()
                    .OrderBy(t => t.Order)
                    .ToList()
                    .ForEach(item => CreateItem().Refresh(item));
            }
            catch (CommunicationException)
            {
                _workspace.AppendErrors(nameof(_workspace.Address), "There was no todo endpoint.");
            }
        }
    }
}
