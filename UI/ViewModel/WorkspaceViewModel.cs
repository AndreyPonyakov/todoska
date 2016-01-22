using System.ComponentModel;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    public sealed class WorkspaceViewModel : BaseViewModel
    {
        private INotifyPropertyChanged _controller;
        private ITodoService _service;

        public INotifyPropertyChanged Controller
        {
            get { return _controller; }
            set { SetField(ref _controller, value); }
        }

        public CategoryControllerViewModel CategoryController { get; }
        public TodoControllerViewModel TodoController { get; }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        public void Update()
        {
            CategoryController.Update(_service.CategoryController);
            TodoController.Update(_service.TodoController);
        }

        /// <summary>
        /// Update command.
        /// </summary>
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// Create instance of <see cref="WorkspaceViewModel"/>.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public WorkspaceViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            UpdateCommand = commandFactory.CreateCommand(Update);
            CategoryController = new CategoryControllerViewModel(commandFactory, service);
            TodoController = new TodoControllerViewModel(commandFactory, service, CategoryController);
            Controller = TodoController;
        }
    }
}
