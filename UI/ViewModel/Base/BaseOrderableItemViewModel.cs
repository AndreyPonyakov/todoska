using System;
using System.Windows.Input;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Implement common functionality of item ViewModel in orderable controller.
    /// </summary>
    /// <typeparam name="TService">Service type. </typeparam>
    /// <typeparam name="TModel">Model type. </typeparam>
    /// <typeparam name="TViewModel">ViewModel type. </typeparam>
    public abstract class BaseOrderableItemViewModel<TService, TModel, TViewModel> 
        : BaseItemViewModel<TService, TModel>, IOrderableItemViewModel<TModel, TViewModel>
        where TViewModel : BaseViewModel
        where TModel : class
    {
        private int _order;
        private bool _orderModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOrderableItemViewModel{TService,TModel,TViewModel}"/> class.
        /// </summary>
        /// <param name="service">Back-end service. </param>
        /// <param name="commandFactory"><see cref="ICommand"/> factory.</param>
        protected BaseOrderableItemViewModel(TService service, ICommandFactory commandFactory)
            : base(service, commandFactory)
        {
            MoveToCommand = CommandFactory.CreateCommand(
                parameter => MoveTo(((DataTransition)parameter).Cast<TViewModel, TViewModel>()));
            this.SetPropertyChanged(nameof(Order), () => OrderModified = true);
        }

        /// <summary>
        /// MoveTo event handler.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type. </typeparam>
        public event MoveToEventHandler<TViewModel, TViewModel> MoveToEvent;

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
        public bool OrderModified
        {
            get { return _orderModified; }
            set { SetField(ref _orderModified, value); }
        }

        /// <summary>
        /// Gets MoveTo command
        /// </summary>
        public ICommand MoveToCommand { get; }

        /// <summary>
        /// MoveTo action.
        /// </summary>
        /// <param name="dataTransition">Transition data. </param>
        public void MoveTo(DataTransition<TViewModel, TViewModel> dataTransition)
        {
            if (dataTransition.Source == dataTransition.Destination)
            {
                return;
            }

            var args = new MoveToEventHandlerArgs<TViewModel, TViewModel>(dataTransition);
            MoveToEvent?.Invoke(this, args);
        }

        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public override void ClearMofidied()
        {
            OrderModified = false;
        }
    }
}
