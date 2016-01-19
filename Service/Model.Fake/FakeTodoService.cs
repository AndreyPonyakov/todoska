using System;
using System.Collections.Generic;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeTodoService : IFakeTodoService
    {
        private static readonly Lazy<FakeTodoService> InstaceLazy;
        public static ITodoService GetInstance() => InstaceLazy.Value;

        public ICategoryController CategoryController { get; }
        public ITodoController TodoController { get; }

        static FakeTodoService()
        {
            InstaceLazy = new Lazy<FakeTodoService>(() => new FakeTodoService());
        }

        public IList<FakeCategory> CategoryList { get; } = new List<FakeCategory>();
        public IList<FakeTodo> TodoList { get; } = new List<FakeTodo>();

        private FakeTodoService()
        {
            CategoryController = new FakeCategoryController(this);
            TodoController = new FakeTodoController(this);
        }
    }
}
