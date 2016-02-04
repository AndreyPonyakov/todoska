using System.Collections.Generic;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Interface for plain table management.
    /// </summary>
    /// <typeparam name="T">Item type. </typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets full item list.
        /// </summary>
        /// <returns>Full item list. </returns>
        /// TODO: Implement IQueryable interface.
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Item instance. </returns>
        T Get(int id);

        /// <summary>
        /// Saves todo into the repository.
        /// </summary>
        /// <param name="item">Saving item. </param>
        /// <returns>Saved item. </returns>
        T Save(T item);

        /// <summary>
        /// Deletes item from the repository.
        /// </summary>
        /// <param name="item">Deleting item. </param>
        void Delete(T item);
    }
}