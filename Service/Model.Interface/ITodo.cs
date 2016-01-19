using System;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Todo class.
    /// </summary>
    public interface ITodo
    {
        int Id { get; }
        string Title { get; }
        string Desc { get; }
        DateTime Deadline { get; }
        int CategoryId { get; }
        bool Checked { get; }
        int Order { get; }

        void Check();
        void SetCategory(int categoryId);
        void SetDeadline(DateTime deadline);
    }
}
