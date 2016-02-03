using System.Collections.Generic;
using System.Linq;
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
        public event MovedEventHandler<TViewModel, TViewModel> Moved;

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
        /// Gets list of attribute properties.
        /// </summary>
        public override IEnumerable<string> Attributes
            => Enumerable.Repeat(nameof(Order), 1).Union(base.Attributes);

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

            var args = new MovedEventHandlerArgs<TViewModel, TViewModel>(dataTransition);
            Moved?.Invoke(this, args);
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
