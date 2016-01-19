using System;
using System.Collections.Generic;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Todo controller class.
    /// </summary>
    public interface ITodoController
    {
        /// <summary>
        /// Select full list of todo from. 
        /// </summary>
        /// <returns>List of Todo. </returns>
        IEnumerable<ITodo> SelectAll();

        /// <summary>
        /// Select single todo instance by id.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        ITodo SelectById(int id);

        /// <summary>
        /// Select list of todo with target title.
        /// </summary>
        /// <param name="title">Target title. </param>
        /// <returns>List of Todo. </returns>
        IEnumerable<ITodo> SelectByTitle(string title);

        /// <summary>
        /// Select list of todo with target category.
        /// </summary>
        /// <param name="categoryId">Target category primary key</param>
        /// <returns>List of Todo. </returns>
        IEnumerable<ITodo> SelectByCategory(int categoryId);

        /// <summary>
        /// Create Todo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="deadline"></param>
        /// <param name="categoryId"></param>
        /// <param name="order"></param>
        /// <returns>Createded todo. </returns>
        ITodo Create(string title, string desc, DateTime deadline, int categoryId, int order);

        /// <summary>
        /// Update todo 
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>Updated todo. </returns>
        ITodo Update(ITodo todo);

        /// <summary>
        /// Delete todo by its id
        /// </summary>
        /// <param name="id">Primary key. </param>
        void Delete(int id);

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority. </param>
        void ChangeOrder(int id, int order);
    }
}
