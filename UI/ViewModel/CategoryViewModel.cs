using System.Drawing;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;
using Todo.UI.ViewModel.Event;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// TODO State Pattern
    /// </summary>
    public sealed class CategoryViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo service. 
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ICategory Model { get; set; }

        #region Notifying properties
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
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

        /// <summary>
        /// Apply action.
        /// </summary>
        public void Apply()
        {
            if (Model == null)
            {
                Model = _service.CategoryController.Create(Name, Color, 0);
                Appended = true;
                Modified = false;
            }
            else
            {
                Model.Name = Name;
                Model.Color = Color;
                Model.Order = Order;
                _service.CategoryController.Update(Model);
                Modified = false;
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
                Modified = false;
            }
            else
            {
                Update(Model);
            }
        }

        /// <summary>
        /// Delete action.
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
        /// MoveTo action.
        /// </summary>
        public void MoveTo(DataTransition<CategoryViewModel, CategoryViewModel> dataTransition)
        {
            if (dataTransition.Source == dataTransition.Destination)
            {
                return;
            }

            var args = new MoveToEventHandlerArgs<CategoryViewModel, CategoryViewModel>(dataTransition);
            MoveToEvent?.Invoke(this, args);
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
        /// MoveTo command
        /// </summary>
        public ICommand MoveToCommand { get; }


        public event MoveToEventHandler<CategoryViewModel, CategoryViewModel> MoveToEvent;

        /// <summary>
        /// Constructor of CategoryViewModel
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            ApplyCommand = commandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = commandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = commandFactory.CreateCommand(Delete, () => CanDelete);
            MoveToCommand = commandFactory.CreateCommand(
                parameter => MoveTo(((DataTransition) parameter).Cast<CategoryViewModel, CategoryViewModel>()));

            this
                .SetPropertyChanged(
                    new[] {nameof(Name), nameof(Color), nameof(Order)},
                    () => Modified = true)
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
        public void Update(ICategory model)
        {
            Model = model;
            Name = model.Name;
            Color = model.Color;
            Modified = false;
        }
    }
}
