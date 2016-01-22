using System.ComponentModel;
using System.Windows.Input;
using TodoSystem.Service.Model.Interface;
using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// Root ViewModel.
    /// </summary>
    public sealed class WorkspaceViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo service.
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// Current controller.
        /// </summary>
        public INotifyPropertyChanged Controller
        {
            get { return _controller; }
            set { SetField(ref _controller, value); }
        }
        private INotifyPropertyChanged _controller;

        /// <summary>
        /// Category controller.
        /// </summary>
        public CategoryControllerViewModel CategoryController { get; }

        /// <summary>
        /// Todo controller.
        /// </summary>
        public TodoControllerViewModel TodoController { get; }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        public void Refresh()
        {
            CategoryController.Refresh(_service.CategoryController);
            TodoController.Refresh(_service.TodoController);
        }

        /// <summary>
        /// Update command.
        /// </summary>
        public ICommand RefreshCommand { get; }


        /// <summary>
        /// Create instance of <see cref="WorkspaceViewModel"/>.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public WorkspaceViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            RefreshCommand = commandFactory.CreateCommand(Refresh);
            CategoryController = new CategoryControllerViewModel(commandFactory, service);
            TodoController = new TodoControllerViewModel(commandFactory, service, CategoryController);
            Controller = TodoController;
        }
    }
}
