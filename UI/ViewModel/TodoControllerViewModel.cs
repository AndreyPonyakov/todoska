using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of main todo controller
    /// </summary>
    public class TodoControllerViewModel : BaseViewModel
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
        /// Category list.
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories { get; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public void CreateCategory()
        {
            var category = Categories.FirstOrDefault(c => c.Model == null);
            if (category == null)
            {
                category = new CategoryViewModel(_commandFactory, _service);
                Categories.Add(category);
                Appending = true;
                category.SetPropertyChanged(nameof(category.Appended), () => Appending = false);
            }
        }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand UndoCreateCategoryCommand { get; set; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public void UndoCreateCategory()
        {
            var category = Categories.FirstOrDefault(c => c.Model == null);
            if (category != null)
            {
                Categories.Remove(category);
                Appending = false;
            }
        }


        /// <summary>
        /// Behind field af Appending
        /// </summary>
        private bool _appending;
        /// <summary>
        /// True if appending new uncommited category
        /// </summary>
        public bool Appending
        {
            get { return _appending; }
            set { SetField(ref _appending, value); }
        }

        /// <summary>
        /// Create insatance if <see cref="TodoControllerViewModel"/>.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        /// <param name="service">Todo service</param>
        public TodoControllerViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            _commandFactory = commandFactory;
            Categories = new ObservableCollection<CategoryViewModel>();
            CreateCategoryCommand = commandFactory.CreateCommand(CreateCategory);
            UndoCreateCategoryCommand = commandFactory.CreateCommand(UndoCreateCategory, () => Appending);
        }
    }
}
