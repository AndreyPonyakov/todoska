using System;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeTodo : ITodo
    {
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

        public FakeTodo(int id)
        {
            Id = id;
            Checked = false;
        }
    }
}
