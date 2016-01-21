using System.Drawing;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Category interface.
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        /// Primary key of category.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Category name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Preferable color of category.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Priority of category.
        /// </summary>
        int Order { get; set; }
    }
}
