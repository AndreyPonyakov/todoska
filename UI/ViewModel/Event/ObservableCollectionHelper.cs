using System;
using System.Collections.ObjectModel;
using System.Linq;

using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Event
{
    /// <summary>
    /// Extension methods for MoveTo event.
    /// </summary>
    public static class ObservableCollectionHelper
    {
        /// <summary>
        /// Implement MoveTo event handler for list.
        /// </summary>
        /// <typeparam name="TSource">Specialization type of list. </typeparam>
        /// <param name="collection">Sender list. </param>
        /// <param name="dataTransition">Transition information of MoveTo event. </param>
        public static void MoveTo<TSource>(this ObservableCollection<TSource> collection, DataTransition<TSource, TSource> dataTransition)
        {
            collection.Move(
                collection.IndexOf(dataTransition.Source),
                collection.IndexOf(dataTransition.Destination));
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the collection. </typeparam>
        /// <typeparam name="TKey">Type of the key as returned by <paramref name="keySelector"/>. </typeparam>
        /// <param name="collection">The collection of values to sort. </param>
        /// <param name="keySelector">A function to extract a key from an element. </param>
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> collection, Func<TSource, TKey> keySelector)
        {
            collection
                .OrderBy(keySelector)
                .Select((item, index) => new { Index = index, Value = item })
                .ToList()
                .ForEach(
                    rec =>
                        {
                            var current = collection.IndexOf(rec.Value);
                            if (current > rec.Index)
                            {
                                collection.Move(current, rec.Index);
                            }
                        });
        }
    }
}
