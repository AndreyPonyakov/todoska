using System;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    /// <summary>
    /// Fake implement of ITodo.
    /// </summary>
    public class FakeTodo : ITodo
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Title of todo.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Deadline of todo.
        /// </summary>
        public DateTime Deadline { get; private set; }

        /// <summary>
        /// Primary key of category.
        /// </summary>
        public int CategoryId { get; private set; }

        /// <summary>
        /// Checked. 
        /// </summary>
        public bool Checked { get; private set; }

        /// <summary>
        /// Order 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Set check.
        /// </summary>
        public void Check(bool isChecked)
        {
            Checked = isChecked;
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
