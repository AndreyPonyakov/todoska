using System.Drawing;

namespace Todo.UI.Model
{
    /// <summary>
    /// Category of Todo
    /// </summary>
    public sealed class Category
    {
        /// <summary>
        /// Name of category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Color of category.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Priority of category.
        /// </summary>
        public int Order { get; set; }
    }
}
