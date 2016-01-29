using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TodoSystem.UI.Model;
using TodoSystem.UI.Model.TodoControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel class of Todo Controller.
    /// </summary>
    /// TODO: Pull members up.
    public sealed class TodoControllerViewModel : BaseOrderedControllerViewModel<ITodoController>
    {
        /// <summary>
        /// Factory for create ICommand instance
        /// </summary>
        private readonly ICommandFactory _commandFactory;

        /// <summary>
        /// ViewModel of category controller
        /// </summary>
        private readonly WorkspaceViewModel _workspace;

        /// <summary>
        /// Todo Service
        /// </summary>
        private ITodoService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoControllerViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand. </param>
        /// <param name="workspace">Workspace with controllers. </param>
        public TodoControllerViewModel(ICommandFactory commandFactory, WorkspaceViewModel workspace)
        {
            _commandFactory = commandFactory;
            _workspace = workspace;

            List = new ObservableCollection<TodoViewModel>();
            CreateCategoryCommand = commandFactory.CreateCommand(() => CreateItem());
            this.SetPropertyChanged(nameof(Service), () => Refresh());
        }

        /// <summary>
        /// Gets or sets back-end service.
        /// </summary>
        public ITodoService Service
        {
            get { return _service; }
            set { SetField(ref _service, value); }
        }

        /// <summary>
        /// Gets value of category list.
        /// </summary>
        public ObservableCollection<TodoViewModel> List { get; }

        /// <summary>
        /// Gets create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; }

        /// <summary>
        /// Create new item in the todo list.
        /// </summary>
        /// <returns>A new instance of the <see cref="TodoControllerClient"> class. </see>/></returns>
        public TodoViewModel CreateItem()
        {
            var todo = new TodoViewModel(_commandFactory, _service, _workspace);
            todo.Order = List.Any() ? List.Max(c => c.Order) : 0;
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
                List.Select((val, i) => new { Value = val, Index = i })
                    .ToList()
                    .ForEach(rec => rec.Value.Order = rec.Index);
            };
            return todo;
        }

        /// <summary>
        /// Update from service.
        /// </summary>
        public void Refresh()
        {
            List.Clear();
            Service.TodoController.SelectAll()
                .OrderBy(t => t.Order)
                .ToList()
                .ForEach(item => CreateItem().Refresh(item));
        }

        /// <summary>
        /// Commit all uncommitted changes.
        /// </summary>
        public void Apply()
        {
            List
                .Where(item => item.CanApply)
                .ToList()
                .ForEach(item => item.Apply());
        }
    }
}
