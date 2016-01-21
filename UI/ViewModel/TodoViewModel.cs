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

        #region Notifying properties
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

        private bool _textModified;
        public bool TextModified
        {
            get { return _textModified; }
            set { SetField(ref _textModified, value); }
        }

        private bool _deadlineModified;
        public bool DeadlineModified
        {
            get { return _deadlineModified; }
            set { SetField(ref _deadlineModified, value); }
        }

        private bool _checkedModified;
        public bool CheckedModified
        {
            get { return _checkedModified; }
            set { SetField(ref _checkedModified, value); }
        }

        private bool _categoryModified;
        public bool CategoryModified
        {
            get { return _categoryModified; }
            set { SetField(ref _categoryModified, value); }
        }

        private bool _modified;
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
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
        #endregion

        public bool InAction => Appended || Canceled || Deleted;
        public bool CanApply => !InAction && (Modified || Model == null);
        public bool CanUndo => !InAction && (Modified || Model == null);
        public bool CanDelete => !InAction && Model != null;

        public ObservableCollection<CategoryViewModel> CategoryList => _categoryController.List;

        /// <summary>
        /// Apply category.
        /// </summary>
        public void Apply()
        {
            if (Model == null)
            {
                if (Category?.Model != null)
                {
                    var categoryId = Category.Model.Id;
                    Model = _service.TodoController.Create(Title, Desc, Deadline, categoryId, Order);
                    Appended = true;

                    TextModified = false;
                    DeadlineModified = false;
                    CheckedModified = false;
                    CategoryModified = false;
                }
            }
            else
            {
                if (TextModified)
                {
                    Model.Title = Title;
                    Model.Desc = Desc;
                    Model.Order = Order;
                    _service.TodoController.Update(Model);
                    TextModified = false;
                }
                if (DeadlineModified)
                {
                    Model.SetDeadline(Deadline);
                    DeadlineModified = false;
                }
                if (CheckedModified)
                {
                    Model.Check();
                    CheckedModified = false;
                }
                if (CategoryModified && Category?.Model != null)
                {
                    Model.SetCategory(Category.Model.Id);
                    CategoryModified = false;
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
                Update(Model);
            }
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
        /// Apply category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand UndoCommand { get; set; }

        /// <summary>
        /// Delete create category command.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// Create <see cref="TodoControllerViewModel"/> instance.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        /// <param name="categoryController">Category controller. </param>
        public TodoViewModel(ICommandFactory commandFactory, ITodoService service,
            CategoryControllerViewModel categoryController)
        {
            _service = service;
            _categoryController = categoryController;
            ApplyCommand = commandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = commandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = commandFactory.CreateCommand(Delete, () => CanDelete);
            this
                .SetPropertyChanged(new[] {nameof(Title), nameof(Desc)}, () => TextModified = true)
                .SetPropertyChanged(nameof(Category), () => CategoryModified = true)
                .SetPropertyChanged(nameof(Deadline), () => DeadlineModified = true)
                .SetPropertyChanged(nameof(Checked), () => CheckedModified = true)
                .SetPropertyChanged(
                    new[]
                    {
                        nameof(TextModified), nameof(CategoryModified),
                        nameof(DeadlineModified), nameof(CheckedModified)
                    },
                    () =>
                    {
                        Modified = TextModified || CategoryModified
                                   || DeadlineModified || CheckedModified;
                    })
                .SetPropertyChanged(
                    new[] {nameof(Appended), nameof(Canceled), nameof(Deleted)},
                    () => OnPropertyChanged(nameof(InAction)))
                .SetPropertyChanged(
                    nameof(InAction),
                    () => OnPropertyChanged(nameof(CanDelete)))
                .SetPropertyChanged(
                    new[] {nameof(InAction), nameof(Modified)},
                    () =>
                    {
                        OnPropertyChanged(nameof(CanApply));
                        OnPropertyChanged(nameof(CanUndo));
                    });
        }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Update(ITodo model)
        {
            Model = model;
            Title = Model.Title;
            Desc = Model.Desc;
            Category = CategoryList.FirstOrDefault(c => c.Model.Id == model.CategoryId);
            Deadline = Model.Deadline;
            Checked = Model.Checked;

            TextModified = false;
            DeadlineModified = false;
            CheckedModified = false;
            CategoryModified = false;
        }

    }
}
