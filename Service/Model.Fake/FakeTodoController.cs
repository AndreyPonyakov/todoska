using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeTodoController : ITodoController
    {
        private readonly IFakeTodoService _service;

        public IEnumerable<ITodo> SelectAll()
        {
            return _service.TodoList;
        }

        public ITodo SelectById(int id)
        {
            return _service.TodoList
                .Single(t => t.Id == id);
        }

        public IEnumerable<ITodo> SelectByTitle(string title)
        {
            return _service.TodoList
                .Where(t => t.Title == title)
                .ToList();
        }

        public IEnumerable<ITodo> SelectByCategory(int categoryId)
        {
            return _service.TodoList
                .Where(t => t.CategoryId == categoryId)
                .ToList();
        }

        public ITodo Create(string title, string desc, DateTime deadline, int categoryId, int order)
        {
            var todo = new FakeTodo(_service, GeterateId())
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

        public void Delete(int id)
        {
            var todo = _service.TodoList
                .FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _service.TodoList.Remove(todo);
            }
        }

        public void ChangeOrder(int id, int order)
        {
            _service.TodoList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Order = order);
        }

        public FakeTodoController(IFakeTodoService service)
        {
            _service = service;
        }

        private int GeterateId()
        {
            return _service.TodoList.Any()
                ? _service.TodoList.Max(c => c.Id) + 1
                : 1;
        }

    }
}
