using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;
using Todo.UI.ViewModel.Base;
using Todo.UI.ViewModel.Event;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of Todo class.
    /// </summary>
    public sealed class TodoViewModel :  BaseOrderedItemViewModel<ITodoService, TodoViewModel>
    {
        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ITodo Model { get; set; }

        /// <summary>
        /// <see cref="CategoryControllerViewModel"/> instance.
        /// </summary>
        private readonly CategoryControllerViewModel _categoryController;

        #region Notifying properties
        
        /// <summary>
        /// Title of current todo.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
        private string _title;
        
        /// <summary>
        /// Description of current todo.
        /// </summary>
        public string Desc
        {
            get { return _desc; }
            set { SetField(ref _desc, value); }
        }
        private string _desc;

        /// <summary>
        /// Deadline of current todo.
        /// </summary>
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetField(ref _deadline, value); }
        }
        private DateTime _deadline = DateTime.Now;

        /// <summary>
        /// Check state of current todo.
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { SetField(ref _checked, value); }
        }
        private bool _checked;

        /// <summary>
        /// Category (ViewModel) of current todo.
        /// </summary>
        private CategoryViewModel _category;
        public CategoryViewModel Category
        {
            get { return _category; }
            set { SetField(ref _category, value); }
        }

        /// <summary>
        /// Change notification flag of title or description.
        /// </summary>
        public bool TextModified
        {
            get { return _textModified; }
            set { SetField(ref _textModified, value); }
        }
        private bool _textModified;

        /// <summary>
        /// Change notification flag of deadline.
        /// </summary>
        public bool DeadlineModified
        {
            get { return _deadlineModified; }
            set { SetField(ref _deadlineModified, value); }
        }
        private bool _deadlineModified;

        /// <summary>
        /// Change notification flag of checked.
        /// </summary>
        public bool CheckedModified
        {
            get { return _checkedModified; }
            set { SetField(ref _checkedModified, value); }
        }
        private bool _checkedModified;

        /// <summary>
        /// Change notification flag of category.
        /// </summary>
        public bool CategoryModified
        {
            get { return _categoryModified; }
            set { SetField(ref _categoryModified, value); }
        }
        private bool _categoryModified;

        /// <summary>
        /// Change notification flag of any property.
        /// </summary>
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
        }
        private bool _modified;

        /// <summary>
        /// Append notification.
        /// </summary>
        /// TODO: implement event.
        public bool Appended
        {
            get { return _appended; }
            set { SetField(ref _appended, value); }
        }
        private bool _appended;

        /// <summary>
        /// Cancel notification.
        /// </summary>
        /// TODO: implement event.
        public bool Canceled
        {
            get { return _canceled; }
            set { SetField(ref _canceled, value); }
        }
        private bool _canceled;

        /// <summary>
        /// Delete notification.
        /// </summary>
        /// TODO: implement event.
        public bool Deleted
        {
            get { return _deleted; }
            set { SetField(ref _deleted, value); }
        }
        private bool _deleted;
        #endregion

        #region Auto-notifying properties
        /// <summary>
        /// Notify any action.
        /// </summary>
        public bool InAction => Appended || Canceled || Deleted;

        /// <summary>
        /// Ability of command execute (apply).
        /// </summary>
        public bool CanApply => !InAction && (Modified || Model == null);

        /// <summary>
        /// Ability of command execute (undo).
        /// </summary>
        public bool CanUndo => !InAction && (Modified || Model == null);

        /// <summary>
        /// Ability of command execute (delete).
        /// </summary>
        public bool CanDelete => !InAction && Model != null;
        #endregion

        /// <summary>
        /// Category list for category selection.
        /// </summary>
        public ObservableCollection<CategoryViewModel> CategoryList => _categoryController.List;

        /// <summary>
        /// Apply action.
        /// </summary>
        public void Apply()
        {
            if (Model == null)
            {
                Create();
            }
            else
            {
                Update(Model);
            }
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        public void Undo()
        {
            if (Model == null)
            {
                Canceled = true;
            }
            else 
            {
                Refresh(Model);
            }
        }

        /// <summary>
        /// Delete action.
        /// </summary>
        public void Delete()
        {
            Service.CategoryController.Delete(Model.Id);
            if (Service.CategoryController.SelectById(Model.Id) == null)
            {
                Deleted = true;
            }
        }

        /// <summary>
        /// Refresh from service.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Refresh(ITodo model)
        {
            Model = model;
            Title = Model.Title;
            Desc = Model.Desc;
            Order = Model.Order;
            Category = CategoryList.FirstOrDefault(c => c.Model.Id == model.CategoryId);
            Deadline = Model.Deadline;
            Checked = Model.Checked;

            ClearMofidied();
        }

        /// <summary>
        /// Update at service.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Update(ITodo model)
        {
            if (TextModified)
            {
                Model.Title = Title;
                Model.Desc = Desc;
                Service.TodoController.Update(Model);
                TextModified = false;
            }
            if (DeadlineModified)
            {
                Model.SetDeadline(Deadline);
                DeadlineModified = false;
            }
            if (CheckedModified)
            {
                Model.Check(Checked);
                CheckedModified = false;
            }
            if (CategoryModified && Category?.Model != null)
            {
                Model.SetCategory(Category.Model.Id);
                CategoryModified = false;
            }
            if (OrderModified)
            {
                Service.TodoController.ChangeOrder(Model.Id, Order);
                OrderModified = false;
            }
        }

        /// <summary>
        /// Create at service.
        /// </summary>
        public void Create()
        {
            if (Category?.Model != null)
            {
                var categoryId = Category.Model.Id;
                Model = Service.TodoController.Create(Title, Desc, Deadline, categoryId, Order);
                ClearMofidied();
                Appended = true;
            }
        }


        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public void ClearMofidied()
        {
            TextModified = false;
            DeadlineModified = false;
            CheckedModified = false;
            CategoryModified = false;
            OrderModified = false;
        }

        /// <summary>
        /// Apply command.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Undo command.
        /// </summary>
        public ICommand UndoCommand { get; }

        /// <summary>
        /// Delete command.
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Create <see cref="TodoControllerViewModel"/> instance.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        /// <param name="categoryController">Category controller. </param>
        public TodoViewModel(ICommandFactory commandFactory, ITodoService service,
            CategoryControllerViewModel categoryController) : base(service, commandFactory)
        {
            _categoryController = categoryController;
            ApplyCommand = CommandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = CommandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = CommandFactory.CreateCommand(Delete, () => CanDelete);

            this
                .SetPropertyChanged(
                    new[] {nameof(Title), nameof(Desc)},
                    () => TextModified = true)
                .SetPropertyChanged(nameof(Category), () => CategoryModified = true)
                .SetPropertyChanged(nameof(Deadline), () => DeadlineModified = true)
                .SetPropertyChanged(nameof(Checked), () => CheckedModified = true)
                .SetPropertyChanged(nameof(Order), () => OrderModified = true)
                .SetPropertyChanged(
                    new[]
                    {
                        nameof(TextModified), nameof(CategoryModified), nameof(OrderModified),
                        nameof(DeadlineModified), nameof(CheckedModified)
                    },
                    () =>
                    {
                        Modified = TextModified || CategoryModified || OrderModified
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

    }
}
