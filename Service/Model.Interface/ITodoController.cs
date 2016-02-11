using System;
using System.Collections.Generic;
using System.ServiceModel;

using TodoSystem.Service.Model.Interface.Faults;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Todo controller class.
    /// </summary>
    [ServiceContract]
    public interface ITodoController
    {
        /// <summary>
        /// Select full list of todo from.
        /// </summary>
        /// <returns>List of Todo. </returns>
        [OperationContract]
        IEnumerable<Todo> SelectAll();

        /// <summary>
        /// Select single todo instance by id.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        [OperationContract]
        Todo SelectById(int id);

        /// <summary>
        /// Select list of todo with target title.
        /// </summary>
        /// <param name="title">Target title. </param>
        /// <returns>List of Todo. </returns>
        [OperationContract]
        IEnumerable<Todo> SelectByTitle(string title);

        /// <summary>
        /// Selects list of todo with target category.
        /// </summary>
        /// <param name="categoryId">Target category primary key</param>
        /// <returns>List of Todo. </returns>
        [OperationContract]
        IEnumerable<Todo> SelectByCategory(int categoryId);

        /// <summary>
        /// Creates a new Todo and appends in the controller.
        /// </summary>
        /// <param name="title">New title value. </param>
        /// <returns>Created todo. </returns>
        [OperationContract]
        [FaultContract(typeof(DataValidationFault))]
        Todo Create(string title);

        /// <summary>
        /// Delete todo by its id
        /// </summary>
        /// <param name="id">Primary key. </param>
        [OperationContract]
        void Delete(int id);

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority in the controller. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        void ChangeOrder(int id, int order);

        /// <summary>
        /// Make checked.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">True if the todo is checked. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        void Check(int id, bool isChecked);

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of new category. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        [FaultContract(typeof(ForeignKeyConstraintFault))]
        void SetCategory(int id, int? categoryId);

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">Deadline time. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        void SetDeadline(int id, DateTime? deadline);

        /// <summary>
        /// Set new title and description by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="title">New title. </param>
        /// <param name="desc">New description. </param>
        [OperationContract]
        [FaultContract(typeof(ItemNotFoundFault))]
        [FaultContract(typeof(DataValidationFault))]
        void ChangeText(int id, string title, string desc);
    }
}
