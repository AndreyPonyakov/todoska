using System.ComponentModel;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    public sealed class WorkspaceViewModel : BaseViewModel
    {
        private INotifyPropertyChanged _controller;
        public INotifyPropertyChanged Controller
        {
            get { return _controller; }
            set { SetField(ref _controller, value); }
        }

        public CategoryControllerViewModel CategoryController { get; }
        public TodoControllerViewModel TodoController { get; }

        public WorkspaceViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            CategoryController = new CategoryControllerViewModel(commandFactory, service);
            TodoController = new TodoControllerViewModel(commandFactory, service, CategoryController);
            Controller = TodoController;
        }
    }
}
