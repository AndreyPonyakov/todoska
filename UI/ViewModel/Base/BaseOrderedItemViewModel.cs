using System.Windows.Input;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Implement common functionality of item ViewModel.
    /// </summary>
    /// <typeparam name="TS">Service type. </typeparam>
    /// <typeparam name="TM">Model type. </typeparam>
    /// <typeparam name="TVM">ViewModel type. </typeparam>
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOrderedItemViewModel<TS, TM, TVM> : BaseViewModel 
        where TVM : BaseViewModel
        where TM : class
    {
        /// <summary>
        /// Back-end service if ViewModel.
        /// </summary>
        protected readonly TS Service;

        /// <summary>
        /// <see cref="ICommandFactory"/> instance.
        /// </summary>
        protected readonly ICommandFactory CommandFactory;

        private int _order;
        private bool _orderModified;
        private bool _modified;

        private bool _canceled;
        private bool _deleted;
        private bool _appended;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOrderedItemViewModel{TS,TVM}"/> class.
        /// </summary>
        /// <param name="service">Back-end service. </param>
        /// <param name="commandFactory"><see cref="ICommand"/> factory.</param>
        protected BaseOrderedItemViewModel(TS service, ICommandFactory commandFactory)
        {
            Service = service;
            CommandFactory = commandFactory;

            ApplyCommand = CommandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = CommandFactory.CreateCommand(Undo, () => CanUndo);
            TryDeleteCommand = CommandFactory.CreateCommand(TryDelete, () => CanDelete);

            MoveToCommand = CommandFactory.CreateCommand(
                parameter => MoveTo(((DataTransition)parameter).Cast<TVM, TVM>()));
            this.SetPropertyChanged(nameof(Order), () => OrderModified = true)
                .SetPropertyChanged(
                    new[]
                        {
                            nameof(Appended), nameof(Canceled), nameof(Deleted)
                        },
                    () => OnPropertyChanged(nameof(InAction)))
                .SetPropertyChanged(
                    nameof(InAction),
                    () => OnPropertyChanged(nameof(CanDelete)))
                .SetPropertyChanged(
                    new[] { nameof(InAction), nameof(Modified) },
                    () =>
                    {
                        OnPropertyChanged(nameof(CanApply));
                        OnPropertyChanged(nameof(CanUndo));
                    });
        }

        /// <summary>
        /// Gets DTO back-end todo.
        /// </summary>
        public TM Model { get; protected set; }


        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of any property.
        /// </summary>
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether append notification.
        /// </summary>
        /// TODO: implement event.
        public bool Appended
        {
            get { return _appended; }
            set { SetField(ref _appended, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether cancel notification.
        /// </summary>
        /// TODO: implement event.
        public bool Canceled
        {
            get { return _canceled; }
            set { SetField(ref _canceled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether delete notification.
        /// </summary>
        /// TODO: implement event.
        public bool Deleted
        {
            get { return _deleted; }
            set { SetField(ref _deleted, value); }
        }

        /// <summary>
        /// MoveTo event handler.
        /// </summary>
        /// <typeparam name="TVM">ViewModel type. </typeparam>
        public event MoveToEventHandler<TVM, TVM> MoveToEvent;

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of priority.
        /// </summary>
        /// 
        public bool OrderModified
        {
            get { return _orderModified; }
            set { SetField(ref _orderModified, value); }
        }

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

        /// <summary>
        /// Gets MoveTo command
        /// </summary>
        public ICommand MoveToCommand { get; }
        /// <summary>
        /// Gets create category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets undo command.
        /// </summary>
        public ICommand UndoCommand { get; }

        /// <summary>
        /// Gets delete  command.
        /// </summary>
        public ICommand TryDeleteCommand { get; protected set; }

        /// <summary>
        /// MoveTo action.
        /// </summary>
        /// <param name="dataTransition">Transition data. </param>
        public void MoveTo(DataTransition<TVM, TVM> dataTransition)
        {
            if (dataTransition.Source == dataTransition.Destination)
            {
                return;
            }

            var args = new MoveToEventHandlerArgs<TVM, TVM>(dataTransition);
            MoveToEvent?.Invoke(this, args);
        }

        /// <summary>
        /// Apply action.
        /// </summary>
        /// TODO: implement event.
        public void Apply()
        {
            if (Model == null)
            {
                Create();
                Appended = true;
                ClearMofidied();
            }
            else
            {
                Update();
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
                ClearMofidied();
            }
        }

        /// <summary>
        /// Attempt complete delete action.
        /// </summary>
        /// TODO: implement event.
        public void TryDelete()
        {
            if (Delete())
            {
                Deleted = true;
            }
        }

        /// <summary>
        /// Delete action.
        /// </summary>
        public abstract bool Delete();

        /// <summary>
        /// Refresh from service.
        /// </summary>
        /// <param name="model">DTO back-end of current todo. </param>
        public virtual void Refresh(TM model)
        {
            ClearMofidied();
        }

        /// <summary>
        /// Update at service.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Create at service.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public abstract bool Create();

        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public virtual void ClearMofidied()
        {
            OrderModified = false;
        }
    }
}
