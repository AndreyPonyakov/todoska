using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel class of Todo Controller.
    /// </summary>
    public sealed class TodoControllerViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo Service
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// Factory for create ICommand instance
        /// </summary>
        private readonly ICommandFactory _commandFactory;

        /// <summary>
        /// ViewModel of category controller
        /// </summary>
        private readonly CategoryControllerViewModel _categoryController;

        /// <summary>
        /// Category list.
        /// </summary>
        public ObservableCollection<TodoViewModel> List { get; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public TodoViewModel CreateItem()
        {
            var todo = new TodoViewModel(_commandFactory, _service, _categoryController);
            List.Add(todo);

            todo
                .SetPropertyChanged(
                    nameof(todo.Appended),
                    () => todo.Appended = false)
                .SetPropertyChanged(
                    nameof(todo.Canceled),
                    () =>
                    {
                        if (todo.Canceled)
                        {
                            List.Remove(todo);
                        }
                        todo.Canceled = false;
                    })
                .SetPropertyChanged(
                    nameof(todo.Deleted),
                    () =>
                    {
                        if (todo.Deleted)
                        {
                            List.Remove(todo);
                        }
                        todo.Deleted = false;
                    });

            return todo;
        }

        /// <summary>
        /// Create <see cref="TodoControllerViewModel"/> instance.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand. </param>
        /// <param name="service">Todo service. </param>
        /// <param name="categoryController">ViewModel of category controller. </param>
        public TodoControllerViewModel(ICommandFactory commandFactory, ITodoService service, 
            CategoryControllerViewModel categoryController)
        {
            _service = service;
            _commandFactory = commandFactory;
            _categoryController = categoryController;

            List = new ObservableCollection<TodoViewModel>();
            CreateCategoryCommand = commandFactory.CreateCommand(() => CreateItem());
        }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Update(ITodoController model)
        {
            List.Clear();
            model.SelectAll()
                .ToList()
                .ForEach(item => CreateItem().Update(item));
        }

    }
}
