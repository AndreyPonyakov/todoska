using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;

using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Class with common functionality of item ViewModel in controller.
    /// </summary>
    /// <typeparam name="TService">Service type. </typeparam>
    /// <typeparam name="TModel">Model type. </typeparam>
    /// TODO: Move messages into resources.
    public abstract class BaseItemViewModel<TService, TModel> : BaseViewModel, IItemViewModel<TModel>
        where TModel : class
    {
        private bool _modified;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseItemViewModel{TService,TModel}"/> class.
        /// </summary>
        /// <param name="service">Back-end service. </param>
        /// <param name="commandFactory"><see cref="ICommand"/> factory.</param>
        protected BaseItemViewModel(TService service, ICommandFactory commandFactory)
        {
            Service = service;
            CommandFactory = commandFactory;

            ApplyCommand = CommandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = CommandFactory.CreateCommand(Undo, () => CanUndo);
            TryDeleteCommand = CommandFactory.CreateCommand(TryDelete, () => CanDelete);

            this.SetPropertyChanged(
                nameof(Modified),
                () =>
                    {
                        OnPropertyChanged(nameof(CanApply));
                        OnPropertyChanged(nameof(CanUndo));
                    });
        }

        /// <summary>
        /// Indicates append notification.
        /// </summary>
        public event EventHandler<EventArgs> Appended;

        /// <summary>
        /// Indicates delete notification.
        /// </summary>
        public event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// On item changed event handler.
        /// </summary>
        public event EventHandler<EventArgs> AttributeChanged;

        /// <summary>
        /// Gets or sets DTO back-end todo.
        /// </summary>
        public TModel Model { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of any property.
        /// </summary>
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
        }

        /// <summary>
        /// Gets list of attribute properties.
        /// </summary>
        public virtual IEnumerable<string> Attributes => Enumerable.Empty<string>();

        /// <summary>
        /// Gets a value indicating whether ability of command execute (apply).
        /// </summary>
        public bool CanApply => Modified || Model == null;

        /// <summary>
        /// Gets a value indicating whether ability of command execute (undo).
        /// </summary>
        public bool CanUndo => Model != null && Modified;

        /// <summary>
        /// Gets a value indicating whether ability of command execute (delete).
        /// </summary>
        public bool CanDelete => true;

        /// <summary>
        /// Gets a value indicating whether service error.
        /// </summary>
        public bool HasServiceError { get; } = false;

        /// <summary>
        /// Gets create category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets undo command.
        /// </summary>
        public ICommand UndoCommand { get; }

        /// <summary>
        /// Gets or sets delete  command.
        /// </summary>
        public ICommand TryDeleteCommand { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether change to refresh notification.
        /// </summary>
        protected bool Refreshing { get; set; }

        /// <summary>
        /// Gets back-end service if ViewModel.
        /// </summary>
        protected TService Service { get; }

        /// <summary>
        /// Gets <see cref="ICommandFactory"/> instance.
        /// </summary>
        protected ICommandFactory CommandFactory { get; }

        /// <summary>
        /// Apply action.
        /// </summary>
        public void Apply()
        {
            ClearErrors(nameof(HasServiceError));

            if (!ContentValidate())
            {
                AppendErrors(nameof(HasServiceError), "Content did not pass validation.");
                return;
            }

            if (Model == null)
            {
                if (Service == null)
                {
                    AppendErrors(nameof(HasServiceError), "Service did not initialize.");
                    return;
                }

                try
                {
                    if (!Create())
                    {
                        AppendErrors(nameof(HasServiceError), "Service cannot append this record.");
                        return;
                    }

                    Appended?.Invoke(this, new EventArgs());
                    if (Modified)
                    {
                        Update();
                    }

                    Refresh(Model);
                }
                catch (FaultException)
                {
                    AppendErrors(nameof(HasServiceError), "Service cannot append this record.");
                }
                catch (CommunicationException)
                {
                    AppendErrors(nameof(HasServiceError), "Service encountered with problems.");
                }
            }
            else
            {
                try
                {
                    Update();
                }
                catch (FaultException)
                {
                    AppendErrors(nameof(HasServiceError), "Service cannot update this record.");
                }
                catch (CommunicationException)
                {
                    AppendErrors(nameof(HasServiceError), "Service encountered with problems.");
                }
            }
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        public void Undo()
        {
            if (Model != null)
            {
                Refresh(Model);
                ClearMofidied();
            }
        }

        /// <summary>
        /// Attempt complete delete action.
        /// </summary>
        public void TryDelete()
        {
            if (Model == null)
            {
                try
                {
                    ClearMofidied();
                    Deleted?.Invoke(this, new EventArgs());
                }
                catch (FaultException)
                {
                    AppendErrors(nameof(HasServiceError), "Service cannot refresh this record.");
                }
                catch (CommunicationException)
                {
                    AppendErrors(nameof(HasServiceError), "Service encountered with problems.");
                }
            }
            else
            {
                ClearErrors(nameof(HasServiceError));
                try
                {
                    if (Delete())
                    {
                        Deleted?.Invoke(this, new EventArgs());
                    }
                }
                catch (FaultException)
                {
                    AppendErrors(nameof(HasServiceError), "Service cannot delete this record.");
                }
                catch (CommunicationException)
                {
                    AppendErrors(nameof(HasServiceError), "Service encountered with problems.");
                }
            }
        }

        /// <summary>
        /// Delete action.
        /// </summary>
        /// <returns>True if records doesn't exist already. </returns>
        public abstract bool Delete();

        /// <summary>
        /// Refresh from service.
        /// </summary>
        /// <param name="model">DTO back-end of current todo. </param>
        public virtual void Refresh(TModel model)
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
        public abstract void ClearMofidied();

        /// <summary>
        /// Checks validation of content properties.
        /// </summary>
        /// <returns>True if content passed validation. </returns>
        public bool ContentValidate() => !this.HasErrors(Attributes);

        /// <summary>
        /// Handles item changed behavior.
        /// </summary>
        protected virtual void ItemChanged()
        {
            if (!Refreshing)
            {
                AttributeChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
