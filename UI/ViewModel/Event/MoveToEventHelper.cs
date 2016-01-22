using System.Collections.Generic;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel.Event
{
    public static class MoveToEventHelper
    {
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
