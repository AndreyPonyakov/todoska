using System;
using System.Collections.Generic;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
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
        /// List of <see cref="ICategory"/> face objects.
        /// </summary>
        public IList<FakeCategory> CategoryList { get; } = new List<FakeCategory>();

        /// <summary>
        /// List of <see cref="ITodo"/> fake objects.
        /// </summary>
        public IList<FakeTodo> TodoList { get; } = new List<FakeTodo>();

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
