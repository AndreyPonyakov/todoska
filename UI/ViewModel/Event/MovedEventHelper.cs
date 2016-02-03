using System.Collections.Generic;
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
        /// <param name="list">Sender list. </param>
        /// <param name="dataTransition">Transition information of MoveTo event. </param>
        public static void MoveTo<T>(this IList<T> list, DataTransition<T, T> dataTransition)
        {
            var source = dataTransition.Source;
            var destination = dataTransition.Destination;

            var sourceIndex = list.IndexOf(source);
            var destinationIndex = list.IndexOf(destination);

            if (sourceIndex != destinationIndex)
            {
                list.Remove(source);
                list.Insert(destinationIndex, source);
            }
        }
    }
}
