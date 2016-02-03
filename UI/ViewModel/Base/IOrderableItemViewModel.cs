using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Interface of functionality of item ViewModel in orderable controller.
    /// </summary>
    /// <typeparam name="TModel">Model type. </typeparam>
    /// <typeparam name="TViewModel">ViewModel type. </typeparam>
    public interface IOrderableItemViewModel<TModel, TViewModel> : IItemViewModel<TModel>
        where TModel : class
        where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Moved event handler.
        /// </summary>
        event MovedEventHandler<TViewModel, TViewModel> Moved;

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// MoveTo action.
        /// </summary>
        /// <param name="dataTransition">Transition data. </param>
        void MoveTo(DataTransition<TViewModel, TViewModel> dataTransition);
    }
}