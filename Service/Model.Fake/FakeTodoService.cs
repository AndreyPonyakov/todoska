using System;
using System.Collections.Generic;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Fake implementation of <see cref="ITodoService"/>.
    /// </summary>
    public sealed class FakeTodoService : IFakeTodoService
    {
        /// <summary>
        /// Backed field of <see cref="ITodoService"/> instance.
        /// Implement lazy singleton pattern. 
        /// </summary>
        private static readonly Lazy<FakeTodoService> InstaceLazy;

        /// <summary>
        /// Singleton instance of <see cref="ITodoService"/>.
        /// </summary>
        public static ITodoService Instance => InstaceLazy.Value;

        /// <summary>
        /// Category controller.
        /// </summary>
        public ICategoryController CategoryController { get; }

        /// <summary>
        /// Todo controller.
        /// </summary>
        public ITodoController TodoController { get; }

        /// <summary>
        /// List of <see cref="Category"/> face objects.
        /// </summary>
        public IList<Category> CategoryList { get; } = new List<Category>();

        /// <summary>
        /// List of <see cref="ITodo"/> fake objects.
        /// </summary>
        public IList<Todo> TodoList { get; } = new List<Todo>();

        public ITodo SelectTodo(Todo todo)
        {
            return new FakeTodo(this, todo);
        }

        /// <summary>
        /// Static constructor of <see cref="FakeTodoService"/>.
        /// </summary>
        static FakeTodoService()
        {
            InstaceLazy = new Lazy<FakeTodoService>(() => new FakeTodoService());
        }

        /// <summary>
        /// Private constructor of <see cref="FakeTodoService"/>.
        /// </summary>
        private FakeTodoService()
        {
            CategoryController = new FakeCategoryController(this);
            TodoController = new FakeTodoController(this);
        }
    }
}
