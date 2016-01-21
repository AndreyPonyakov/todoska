using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of Todo class
    /// </summary>
    public sealed class TodoViewModel : BaseViewModel
    {
        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ITodo Model { get; set; }

        private readonly ITodoService _service;
        private readonly CategoryControllerViewModel _categoryController;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        private string _desc;
        public string Desc
        {
            get { return _desc; }
            set { SetField(ref _desc, value); }
        }

        private DateTime _deadline = DateTime.Now;
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetField(ref _deadline, value); }
        }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { SetField(ref _checked, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }

        private CategoryViewModel _category;
        public CategoryViewModel Category
        {
            get { return _category; }
            set { SetField(ref _category, value); }
        }

        private bool _modified;
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
        }

        private bool _deadlineModified;
        public bool DeadlineModified
        {
            get { return _deadlineModified; }
            set { SetField(ref _deadlineModified, value); }
        }

        private bool _appended;
        public bool Appended
        {
            get { return _appended; }
            set { SetField(ref _appended, value); }
        }

        private bool _canceled;
        public bool Canceled
        {
            get { return _canceled; }
            set { SetField(ref _canceled, value); }
        }

        private bool _deleted;

        public bool Deleted
        {
            get { return _deleted; }
            set { SetField(ref _deleted, value); }
        }

        public bool InAction => Appended || Canceled || Deleted;
        public bool CanApply => !InAction && (Modified || DeadlineModified || Model == null);
        public bool CanUndo => !InAction && (Modified || DeadlineModified || Model == null);
        public bool CanDelete => !InAction && Model != null;

        public ObservableCollection<CategoryViewModel> CategoryList => _categoryController.List;


        /// <summary>
        /// Finish of creating category.
        /// </summary>
        public void Apply()
        {
            if (Model == null)
            {
                if (Category.Model != null)
                {
                    var categoryId = Category.Model.Id;
                    Model = _service.TodoController.Create(Title, Desc, Deadline, categoryId, Order);
                    Appended = true;

                    Modified = false;
                    DeadlineModified = false;
                }
            }
            else
            {
                if (Modified)
                {
                    _service.TodoController.Update(Model);
                    Modified = false;
                }
                if (DeadlineModified)
                {
                    Model.SetDeadline(Deadline);
                    DeadlineModified = false;
                }
            }
        }

        /// <summary>
        /// Create category command.
        /// </summary>
        public void Undo()
        {
            if (Model == null)
            {
                Canceled = true;
            }
            else 
            {
                if (Modified)
                {
                    Title = Model.Title;
                    Desc = Model.Desc;
                }
                if (DeadlineModified)
                {
                    Deadline = Model.Deadline;
                    Category = CategoryList.FirstOrDefault(c => c.Model.Id == Model.Id);
                }
            }
            Modified = false;
            DeadlineModified = false;
        }

        /// <summary>
        /// Create category command.
        /// </summary>
        public void Delete()
        {
            _service.CategoryController.Delete(Model.Id);
            if (_service.CategoryController.SelectById(Model.Id) == null)
            {
                Deleted = true;
            }
        }

        /// <summary>
        /// Create category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand UndoCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        public TodoViewModel(ICommandFactory commandFactory, ITodoService service, 
            CategoryControllerViewModel categoryController)
        {
            _service = service;
            _categoryController = categoryController;
            ApplyCommand = commandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = commandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = commandFactory.CreateCommand(Delete, () => CanDelete);
            this
                .SetPropertyChanged(
                    new[] { nameof(Title), nameof(Desc)},
                    () => Modified = true)
                .SetPropertyChanged(
                    new[] { nameof(Deadline) },
                    () => DeadlineModified = true)
                .SetPropertyChanged(
                    new[] { nameof(Appended), nameof(Canceled), nameof(Deleted) },
                    () => OnPropertyChanged(nameof(InAction)))
                .SetPropertyChanged(
                    nameof(InAction),
                    () => OnPropertyChanged(nameof(CanDelete)))
                .SetPropertyChanged(
                    new[] { nameof(InAction), nameof(Modified), nameof(DeadlineModified) },
                    () =>
                    {
                        OnPropertyChanged(nameof(CanApply));
                        OnPropertyChanged(nameof(CanUndo));
                    });
        }

    }
}
