using System.Drawing;
using Todo.Service.Model.Interface;

namespace Todo.UI.Model
{
    /// <summary>
    /// Category of Todo
    /// </summary>
    public sealed class Category
    {
        public ICategory Remote { get; }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int? Id { get; }

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

        /// <summary>
        /// Create instance <see cref="Category"/>
        /// </summary>
        /// <param name="remoteCategory">Remoted category instance. </param>
        public Category(ICategory remoteCategory)
        {
            Remote = remoteCategory;
            Id = remoteCategory.Id;
            Name = remoteCategory.Name;
            Color = remoteCategory.Color;
            Order = remoteCategory.Order;
        }

        /// <summary>
        /// Create instance <see cref="Category"/>
        /// </summary>
        public Category()
        {
        }
    }
}
