using System.Collections.Generic;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public interface IFakeTodoService : ITodoService
    {
        IList<FakeCategory> CategoryList { get; }
        IList<FakeTodo> TodoList { get; }
    }
}
