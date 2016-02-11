using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

using TodoSystem.Service.Model.Interface.Faults;

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
        /// <returns>Category instance. </returns>
        [OperationContract]
        [FaultContract(typeof(DataValidationFault))]
        Category Create(string name);

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        [OperationContract]
        [FaultContract(typeof(ForeignKeyConstraintFault))]
        void Delete(int id);

        /// <summary>
        /// Change name of item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="name">Target name. </param>
        [OperationContract]
        [FaultContract(typeof(DataValidationFault))]
        [FaultContract(typeof(ItemNotFoundFault))]
        void ChangeText(int id, string name);

        /// <summary>
        /// Change priority in the list.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Target priority in list. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        void ChangeOrder(int id, int order);

        /// <summary>
        /// Change color of item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="color">Target color. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        void ChangeColor(int id, Color? color);
    }
}
