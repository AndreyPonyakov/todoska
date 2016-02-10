using System.Collections.ObjectModel;

using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Event
{
    /// <summary>
    /// Extension methods for MoveTo event.
    /// </summary>
    public static class MovedEventHelper
    {
        /// <summary>
        /// Implement MoveTo event handler for list.
        /// </summary>
        /// <typeparam name="T">Specialization type of list. </typeparam>
        /// <param name="collection">Sender list. </param>
        /// <param name="dataTransition">Transition information of MoveTo event. </param>
        public static void MoveTo<T>(this ObservableCollection<T> collection, DataTransition<T, T> dataTransition)
        {
            collection.Move(
                collection.IndexOf(dataTransition.Source),
                collection.IndexOf(dataTransition.Destination));
        }
    }
}
