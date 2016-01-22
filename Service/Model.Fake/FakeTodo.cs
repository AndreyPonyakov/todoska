using System;
using System.Linq;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Fake implement of ITodo.
    /// </summary>
    public sealed class FakeTodo : ITodo
    {
        
        private readonly int _id;
        private readonly FakeTodoService _service;

        /// <summary>
        /// Set check.
        /// </summary>
        public void Check(bool isChecked)
        {
            _service.TodoList
                .Where(t => t.Id == _id)
                .ToList()
                .ForEach(t => t.Checked = isChecked);
        }

        /// <summary>
        /// Change of category.
        /// </summary>
        /// <param name="categoryId">Primary key of category. </param>
        public void SetCategory(int categoryId)
        {
            _service.TodoList
                .Where(t => t.Id == _id)
                .ToList()
                .ForEach(t => t.CategoryId = categoryId);
        }

        /// <summary>
        /// Change of deadline.
        /// </summary>
        /// <param name="deadline">New deadline. </param>
        public void SetDeadline(DateTime deadline)
        {
            _service.TodoList
                .Where(t => t.Id == _id)
                .ToList()
                .ForEach(t => t.Deadline = deadline);
        }

        /// <summary>
        /// Create <see cref="FakeTodo"/> instance. 
        /// </summary>
        /// <param name="service">Fake Todo service.</param>
        /// <param name="todo">Todo instance. </param>
        public FakeTodo(FakeTodoService service, Todo todo)
        {
            _service = service;
            _id = todo.Id;
        }
    }
}
