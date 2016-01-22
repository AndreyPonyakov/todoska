using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TodoSystem.Service.Model.Interface;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel class of Todo Controller.
    /// </summary>
    public sealed class TodoControllerViewModel : BaseOrderedControllerViewModel
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
            todo.Order = (List.Any() ? List.Max(c => c.Order) : 0) + 1;
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

            todo.MoveToEvent += (sender, args) =>
            {
                List.MoveTo(args.DataTransition);
                for(var i = 0;  i < List.Count; i++)
                {
                    List[i].Order = i;
                }
            };
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
        public void Refresh(ITodoController model)
        {
            List.Clear();
            foreach (var item in model.SelectAll())
            {
                CreateItem().Refresh(item);
            }

        }

    }
}
