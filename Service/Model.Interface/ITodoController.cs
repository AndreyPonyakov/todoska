using System;
using System.Collections.Generic;
using System.ServiceModel;

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
        /// <param name="desc">New description value. </param>
        /// <param name="deadline">New deadline value. </param>
        /// <param name="categoryId">New category primary key. </param>
        /// <param name="order">New priority value. </param>
        /// <returns>Created todo. </returns>
        [OperationContract]
        Todo Create(string title, string desc, DateTime? deadline, int categoryId, int order);

        /// <summary>
        /// Updates from other DTO todo instance.
        /// </summary>
        /// <param name="todo">Updated todo. </param>
        [OperationContract]
        void Update(Todo todo);

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
        void ChangeOrder(int id, int order);

        /// <summary>
        /// Make checked.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">True if the todo is checked. </param>
        [OperationContract]
        void Check(int id, bool isChecked);

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of new category. </param>
        [OperationContract]
        void SetCategory(int id, int categoryId);

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">Deadline time. </param>
        [OperationContract]
        void SetDeadline(int id, DateTime? deadline);
    }
}
