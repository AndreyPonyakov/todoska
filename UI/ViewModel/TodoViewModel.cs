using System;
using System.Collections.ObjectModel;
using System.Linq;
using TodoSystem.UI.Model;
using TodoSystem.UI.Model.TodoControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel of Todo class.
    /// </summary>
    public sealed class TodoViewModel : BaseOrderedItemViewModel<ITodoService, Todo, TodoViewModel>
    {
        /// <summary>
        /// <see cref="CategoryControllerViewModel"/> instance.
        /// </summary>
        private readonly CategoryControllerViewModel _categoryController;

        private string _title;
        private string _desc;
        private DateTime _deadline = DateTime.Now;
        private bool _checked;

        private bool _textModified;
        private bool _deadlineModified;
        private bool _categoryModified;
        private bool _checkedModified;

        private CategoryViewModel _category;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for command instance. </param>
        /// <param name="service">Todo service. </param>
        /// <param name="workspace">Workspace with controllers. </param>
        public TodoViewModel(ICommandFactory commandFactory, ITodoService service, WorkspaceViewModel workspace)
            : base(service, commandFactory)
        {
            _categoryController = workspace.CategoryController;

            this.SetPropertyChanged(
                new[] { nameof(Title), nameof(Desc) },
                () => TextModified = true)
                .SetPropertyChanged(
                    nameof(Category),
                    () => CategoryModified = true)
                .SetPropertyChanged(
                    nameof(Deadline),
                    () => DeadlineModified = true)
                .SetPropertyChanged(
                    nameof(Checked),
                    () => CheckedModified = true)
                .SetPropertyChanged(
                    new[]
                        {
                            nameof(TextModified), nameof(CategoryModified),
                            nameof(OrderModified), nameof(DeadlineModified),
                            nameof(CheckedModified)
                        },
                    () =>
                        {
                            Modified = TextModified || CategoryModified
                                       || OrderModified || DeadlineModified
                                       || CheckedModified;
                        });

        }

        /// <summary>
        /// Gets or sets a title of current todo.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
        
        /// <summary>
        /// Gets or sets a description of current todo.
        /// </summary>
        public string Desc
        {
            get { return _desc; }
            set { SetField(ref _desc, value); }
        }

        /// <summary>
        /// Gets or sets a deadline of current todo.
        /// </summary>
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetField(ref _deadline, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether check state of current todo.
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { SetField(ref _checked, value); }
        }

        /// <summary>
        /// Gets or sets a category (ViewModel) of current todo.
        /// </summary>
        public CategoryViewModel Category
        {
            get { return _category; }
            set { SetField(ref _category, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of title or description.
        /// </summary>
        public bool TextModified
        {
            get { return _textModified; }
            set { SetField(ref _textModified, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of deadline.
        /// </summary>
        public bool DeadlineModified
        {
            get { return _deadlineModified; }
            set { SetField(ref _deadlineModified, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of checked.
        /// </summary>
        public bool CheckedModified
        {
            get { return _checkedModified; }
            set { SetField(ref _checkedModified, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of category.
        /// </summary>
        public bool CategoryModified
        {
            get { return _categoryModified; }
            set { SetField(ref _categoryModified, value); }
        }
        
        /// <summary>
        /// Category list for category selection.
        /// </summary>
        public ObservableCollection<CategoryViewModel> CategoryList => _categoryController.List;

        /// <summary>
        /// Delete action.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public override bool Delete()
        {
            Service.CategoryController.Delete(Model.Id);
            return Service.CategoryController.SelectById(Model.Id) == null;
        }

        /// <summary>
        /// Refresh from service.
        /// </summary>
        /// <param name="model">DTO back-end of current todo. </param>
        public override void Refresh(Todo model)
        {
            Model = model;
            Title = Model.Title;
            Desc = Model.Desc;
            Order = Model.Order;
            Category = CategoryList.FirstOrDefault(c => c.Model.Id == model.CategoryId);
            Deadline = Model.Deadline;
            Checked = Model.Checked;
            Service.TodoController.Update(Model);
            base.Refresh(model);
        }

        /// <summary>
        /// Update at service.
        /// </summary>
        public override void Update()
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
                Service.TodoController.SetDeadline(Model.Id, Deadline);
                Model.Deadline = Deadline;
                DeadlineModified = false;
            }

            if (CheckedModified)
            {
                Service.TodoController.Check(Model.Id, Checked);
                Model.Checked = Checked; 
                CheckedModified = false;
            }

            if (CategoryModified && Category?.Model != null)
            {
                Service.TodoController.SetCategory(Model.Id, Category.Model.Id);
                Model.CategoryId = Category.Model.Id;
                CategoryModified = false;
            }

            if (OrderModified)
            {
                Service.TodoController.ChangeOrder(Model.Id, Order);
                Model.Order = Order;
                OrderModified = false;
            }
        }

        /// <summary>
        /// Create at service.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public override bool Create()
        {
            if (Category?.Model != null)
            {
                var categoryId = Category.Model.Id;
                Model = Service.TodoController.Create(Title, Desc, Deadline, categoryId, Order);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public override void ClearMofidied()
        {
            base.ClearMofidied();
            TextModified = false;
            DeadlineModified = false;
            CheckedModified = false;
            CategoryModified = false;
            OrderModified = false;
        }
    }
}
