using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    /// <summary>
    /// Fake implementation of ITodoController.
    /// </summary>
    public sealed class FakeTodoController : ITodoController
    {
        /// <summary>
        /// Todo service.
        /// </summary>
        private readonly IFakeTodoService _service;

        /// <summary>
        /// Fetch all todo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITodo> SelectAll()
        {
            return _service.TodoList;
        }

        /// <summary>
        /// Get todo by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITodo SelectById(int id)
        {
            return _service.TodoList
                .FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Get all "todo"es with target title.
        /// </summary>
        /// <param name="title">Target key. </param>
        /// <returns></returns>
        public IEnumerable<ITodo> SelectByTitle(string title)
        {
            return _service.TodoList
                .Where(t => t.Title == title)
                .ToList();
        }

        /// <summary>
        /// Get all "todo"es with target category.
        /// </summary>
        /// <param name="categoryId">Primary key of category. </param>
        /// <returns></returns>
        public IEnumerable<ITodo> SelectByCategory(int categoryId)
        {
            return _service.TodoList
                .Where(t => t.CategoryId == categoryId)
                .ToList();
        }

        /// <summary>
        /// Create new todo.
        /// </summary>
        /// <param name="title">Todo title. </param>
        /// <param name="desc">Todo description. </param>
        /// <param name="deadline">Todo deadline. </param>
        /// <param name="categoryId">Primary key of category. </param>
        /// <param name="order">Priority. </param>
        /// <returns></returns>
        public ITodo Create(string title, string desc, DateTime deadline, int categoryId, int order)
        {
            var todo = new FakeTodo(GeterateId())
            {
                Title = title,
                Desc = desc,
                Order = order
            };
            todo.SetCategory(categoryId);
            todo.SetDeadline(deadline);
            _service.TodoList.Add(todo);
            return todo;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="todo">New todo </param>
        public void Update(ITodo todo)
        {
            _service.TodoList
                .Where(t => t.Id == todo.Id)
                .ToList()
                .ForEach(t =>
                {
                    t.Title = todo.Title;
                    t.Desc = todo.Desc;
                    t.Order = todo.Order;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
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
        /// Cahnge priority.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority. </param>
        public void ChangeOrder(int id, int order)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Order = order);
        }

        /// <summary>
        /// Create <see cref="FakeTodoController"/> instance. 
        /// </summary>
        /// <param name="service">Service of data storage. </param>
        public FakeTodoController(IFakeTodoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Generate new primary key.
        /// </summary>
        /// <returns></returns>
        private int GeterateId()
        {
            return _service.TodoList.Any()
                ? _service.TodoList.Max(c => c.Id) + 1
                : 1;
        }

    }
}
