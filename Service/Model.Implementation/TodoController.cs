using System;
using System.Collections.Generic;

using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.Implementation
{
    /// <summary>
    /// Storage implementation for <see cref="ITodoController"/>
    /// </summary>
    public sealed class TodoController : ITodoController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoController"/> class.
        /// </summary>
        /// <param name="repository">Category repository. </param>
        public TodoController(ITodoRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            Repository = repository;
        }

        private ITodoRepository Repository { get; }

        /// <summary>
        /// Select full list of todo from.
        /// </summary>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Todo> SelectAll() => Repository.GetAll();

        /// <summary>
        /// Select single todo instance by id.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        public Todo SelectById(int id) => Repository.Get(id);

        /// <summary>
        /// Select list of todo with target title.
        /// </summary>
        /// <param name="title">Target title. </param>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Todo> SelectByTitle(string title) => Repository.Find(title);

        /// <summary>
        /// Selects list of todo with target category.
        /// </summary>
        /// <param name="categoryId">Target category primary key</param>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Todo> SelectByCategory(int categoryId)
        {
            var category = new Category { Id = categoryId };
            return Repository.Find(category);
        }

        /// <summary>
        /// Creates a new Todo and appends in the controller.
        /// </summary>
        /// <param name="title">New title value. </param>
        /// <param name="desc">New description value. </param>
        /// <param name="deadline">New deadline value. </param>
        /// <param name="categoryId">New category primary key. </param>
        /// <param name="order">New priority value. </param>
        /// <returns>Created todo. </returns>
        public Todo Create(string title, string desc, DateTime? deadline, int categoryId, int order)
        {
            var todo = new Todo
            {
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Order = order
            };
            return Repository.Save(todo);
        }

        /// <summary>
        /// Updates from other DTO todo instance.
        /// </summary>
        /// <param name="todo">Updated todo. </param>
        public void Update(Todo todo) => Repository.Save(todo);

        /// <summary>
        /// Delete todo by its id
        /// </summary>
        /// <param name="id">Primary key. </param>
        public void Delete(int id) => Repository.Delete(Repository.Get(id));

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority in the controller. </param>
        public void ChangeOrder(int id, int order)
        {
            var todo = Repository.Get(id);
            todo.Order = order;
            Repository.Save(todo);
        }

        /// <summary>
        /// Make checked.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">True if the todo is checked. </param>
        public void Check(int id, bool isChecked)
        {
            var todo = Repository.Get(id);
            todo.Checked = isChecked;
            Repository.Save(todo);
        }

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of new category. </param>
        public void SetCategory(int id, int categoryId)
        {
            var todo = Repository.Get(id);
            todo.CategoryId = categoryId;
            Repository.Save(todo);
        }

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">Deadline time. </param>
        public void SetDeadline(int id, DateTime? deadline)
        {
            var todo = Repository.Get(id);
            todo.Deadline = deadline;
            Repository.Save(todo);
        }
    }
}
