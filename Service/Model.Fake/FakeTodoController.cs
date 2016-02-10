using System;
using System.Collections.Generic;
using System.Linq;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Fake implementation of ITodoController.
    /// </summary>
    public class FakeTodoController : ITodoController
    {
        /// <summary>
        /// Todo service.
        /// </summary>
        private readonly IStorageService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeTodoController"/> class.
        /// </summary>
        public FakeTodoController()
        {
            _service = FakeStorageService.Instance;
        }

        /// <summary>
        /// Fetch all todo.
        /// </summary>
        /// <returns>List of selected todo.</returns>
        public IEnumerable<Todo> SelectAll()
        {
            return _service.TodoList;
        }

        /// <summary>
        /// Get todo by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Selected todo. </returns>
        public Todo SelectById(int id)
        {
            return _service.TodoList
                .FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Gets all todo with target title.
        /// </summary>
        /// <param name="title">Target key. </param>
        /// <returns>List of selected todo.</returns>
        public IEnumerable<Todo> SelectByTitle(string title)
        {
            return _service.TodoList
                .Where(t => t.Title == title);
        }

        /// <summary>
        /// Gets all todo with target category.
        /// </summary>
        /// <param name="categoryId">Primary key of category. </param>
        /// <returns>List of selected todo.</returns>
        public IEnumerable<Todo> SelectByCategory(int categoryId)
        {
            return _service.TodoList
                .Where(t => t.CategoryId == categoryId);
        }

        /// <summary>
        /// Create new todo.
        /// </summary>
        /// <param name="title">Todo title. </param>
        /// <returns>Created todo. </returns>
        public Todo Create(string title)
        {
            var todo = new Todo
            {
                Id = GeterateId(),
                Title = title,
                Desc = null,
                Order = GeterateId(),
                CategoryId = null,
                Deadline = null
            };
            _service.TodoList.Add(todo);
            return todo;
        }

        /// <summary>
        /// Deletes todo by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        public void Delete(int id)
        {
            var todo = _service.TodoList
                .FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _service.TodoList.Remove(todo);
            }
        }

        /// <summary>
        /// Changes priority.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority in the controllers. </param>
        public void ChangeOrder(int id, int order)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Order = order);
        }

        /// <summary>
        /// Sets checked state.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">Checked state. </param>
        public void Check(int id, bool isChecked)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Checked = isChecked);
        }

        /// <summary>
        /// Change of category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of category. </param>
        public void SetCategory(int id, int? categoryId)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.CategoryId = categoryId);
        }

        /// <summary>
        /// Change of deadline.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">New deadline. </param>
        public void SetDeadline(int id, DateTime? deadline)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Deadline = deadline);
        }

        /// <summary>
        /// Set new title and description by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="title">New title. </param>
        /// <param name="desc">New description. </param>
        public void ChangeText(int id, string title, string desc)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(
                    t =>
                        {
                            t.Title = title;
                            t.Desc = desc;
                        });
        }

        /// <summary>
        /// Generate new primary key.
        /// </summary>
        /// <returns>New primary key. </returns>
        private int GeterateId()
        {
            return _service.TodoList.Any()
                ? _service.TodoList.Max(c => c.Id) + 1
                : 1;
        }
    }
}
