using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Category controller class.
    /// </summary>
    [ServiceContract]
    public interface ICategoryController
    {
        /// <summary>
        /// Gets full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        [OperationContract]
        IEnumerable<Category> SelectAll();

        /// <summary>
        /// Fetches single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        [OperationContract]
        Category SelectById(int id);

        /// <summary>
        /// Fetches category list with target name. 
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        [OperationContract]
        IEnumerable<Category> SelectByName(string name);

        /// <summary>
        /// Creates new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">New priority. </param>
        /// <returns>Category instance. </returns>
        [OperationContract]
        Category Create(string name, Color color, int order);

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        [OperationContract]
        void Update(Category category);

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        [OperationContract]
        void Delete(int id);

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Target priority in list. </param>
        [OperationContract]
        void ChangeOrder(int id, int order);
    }
}
