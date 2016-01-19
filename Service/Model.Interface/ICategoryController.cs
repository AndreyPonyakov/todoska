using System.Collections.Generic;
using System.Drawing;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Category controller class.
    /// </summary>
    public interface ICategoryController
    {
        /// <summary>
        /// Get full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        IEnumerable<ICategory> SelectAll();

        /// <summary>
        /// Get single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        ICategory SelectById(int id);

        /// <summary>
        /// Get category list with target name. 
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        IEnumerable<ICategory> SelectByName(string name);

        /// <summary>
        /// Create new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">Priority. </param>
        /// <returns>Category instance. </returns>
        ICategory Create(string name, Color color, int order);

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        void Update(ICategory category);

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        void Delete(int id);

    }
}
