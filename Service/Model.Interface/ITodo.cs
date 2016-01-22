using System;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Todo class.
    /// </summary>
    public interface ITodo
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Short title text.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Long text body.
        /// </summary>
        string Desc { get; set; }

        /// <summary>
        /// Deadline date.
        /// </summary>
        DateTime Deadline { get; }

        /// <summary>
        /// Primary key of compounded category.
        /// </summary>
        int CategoryId { get; }

        /// <summary>
        /// Checked state.
        /// </summary>
        bool Checked { get; }

        /// <summary>
        /// Priority.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Make checked.
        /// </summary>
        void Check(bool isChecked);

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="categoryId">Primary key of new category. </param>
        void SetCategory(int categoryId);

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="deadline">Deadline time. </param>
        void SetDeadline(DateTime deadline);
    }
}
