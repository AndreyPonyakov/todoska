using System.Collections.Generic;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Interface for plain category table management.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Filters category list by category.
        /// </summary>
        /// <param name="name">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        IEnumerable<Category> Find(string name);

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        Category FindLast();
    }
}
