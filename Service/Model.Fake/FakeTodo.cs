using System;
using System.Linq;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeTodo : ITodo
    {
        private readonly IFakeTodoService _service;
        public int Id { get; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime Deadline { get; private set; }
        public int CategoryId { get; private set; }
        public bool Checked { get; private set; }
        public int Order { get; set; }
        public void Check()
        {
            Checked = true;
        }

        public void SetCategory(int categoryId)
        {
            CategoryId = categoryId;
        }

        public void SetDeadline(DateTime deadline)
        {
            Deadline = deadline;
        }

        public FakeTodo(IFakeTodoService service, int id)
        {
            _service = service;
            Id = id;
            Checked = false;
        }
    }
}
