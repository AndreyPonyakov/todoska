using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;
using Todo.UI.ViewModel.Event;

namespace Todo.UI.ViewModel.Base
{
    /// <summary>
    /// Implement common functionality of item ViewModel.
    /// </summary>
    /// <typeparam name="TS">Service type. </typeparam>
    /// <typeparam name="TVM">ViewModel type. </typeparam>
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOrderedItemViewModel<TS, TVM> : BaseViewModel 
        where TVM : BaseViewModel
    {

        /// <summary>
        /// Priority.
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }
        private int _order;

        /// <summary>
        /// Change notification flag of priority.
        /// </summary>
        public bool OrderModified
        {
            get { return _orderModified; }
            set { SetField(ref _orderModified, value); }
        }
        private bool _orderModified;

        /// <summary>
        /// MoveTo event handler.
        /// </summary>
        /// <typeparam name="TVM">ViewModel type. </typeparam>
        public event MoveToEventHandler<TVM, TVM> MoveToEvent;

        /// <summary>
        /// MoveTo action.
        /// </summary>
        /// <typeparam name="TVM">ViewModel type. </typeparam>
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
        /// MoveTo command
        /// </summary>
        public ICommand MoveToCommand { get; }

        /// <summary>
        /// <see cref="ITodoService"/> instance.
        /// </summary>
        protected readonly TS Service;

        /// <summary>
        /// <see cref="ICommandFactory"/> instance.
        /// </summary>
        protected readonly ICommandFactory CommandFactory;

        protected BaseOrderedItemViewModel(TS service, ICommandFactory commandFactory)
        {
            Service = service;
            CommandFactory = commandFactory;

            MoveToCommand = CommandFactory.CreateCommand(
                parameter => MoveTo(((DataTransition)parameter).Cast<TVM, TVM>()));

        }
    }
}
