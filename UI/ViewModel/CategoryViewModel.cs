using System.Drawing;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;
using Todo.UI.ViewModel.Base;
using Todo.UI.ViewModel.Event;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// TODO State Pattern
    /// </summary>
    public sealed class CategoryViewModel : BaseOrderedItemViewModel<ITodoService, CategoryViewModel>
    {
        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ICategory Model { get; set; }

        #region Notifying properties
        
        /// <summary>
        /// Category name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }
        private string _name;

        /// <summary>
        /// Category color.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }
        private Color _color;

        /// <summary>
        /// Change notification flag of name or color.
        /// </summary>
        public bool DataModified
        {
            get { return _dataModified; }
            set { SetField(ref _dataModified, value); }
        }
        private bool _dataModified;

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
        /// Apply action.
        /// </summary>
        /// TODO: implement event.
        public void Apply()
        {
            if (Model == null)
            {
                Model = Service.CategoryController.Create(Name, Color, 0);
                Appended = true;
                DataModified = false;
                OrderModified = false;
            }
            else
            {
                if (DataModified)
                {
                    Model.Name = Name;
                    Model.Color = Color;
                    Service.CategoryController.Update(Model);
                    DataModified = false;
                }
                if (OrderModified)
                {
                    Service.CategoryController.ChangeOrder(Model.Id, Order);
                    OrderModified = false;
                }
            }
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        /// TODO: implement event.
        public void Undo()
        {
            if (Model == null)
            {
                Canceled = true;
                ClearMofidied();
            }
            else
            {
                Refresh(Model);
            }
        }

        /// <summary>
        /// Delete action.
        /// </summary>
        /// TODO: implement event.
        public void Delete()
        {
            Service.CategoryController.Delete(Model.Id);
            if (Service.CategoryController.SelectById(Model.Id) == null)
            {
                Deleted = true;
            }
        }

        /// <summary>
        /// Create category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Undo command.
        /// </summary>
        public ICommand UndoCommand { get;  }

        /// <summary>
        /// Delete  command.
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Refresh(ICategory model)
        {
            Model = model;
            Name = model.Name;
            Color = model.Color;
            Order = model.Order;
            ClearMofidied();
        }

        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public void ClearMofidied()
        {
            DataModified = false;
            OrderModified = false;
        }

        /// <summary>
        /// Constructor of CategoryViewModel
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service) : base(service, commandFactory)
        {
            ApplyCommand = commandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = commandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = commandFactory.CreateCommand(Delete, () => CanDelete);

            this
                .SetPropertyChanged(
                    new[] {nameof(Name), nameof(Color)}, () => DataModified = true)
                .SetPropertyChanged(
                    nameof(Order), () => OrderModified = true)
                .SetPropertyChanged(
                    new[] { nameof(OrderModified), nameof(DataModified) },
                    () =>
                    {
                        Modified = DataModified || OrderModified;
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
