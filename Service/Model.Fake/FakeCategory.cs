using System.Drawing;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeCategory : ICategory
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Priority.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Create <see cref="FakeCategory" /> instance. 
        /// </summary>
        /// <param name="id"></param>
        public FakeCategory(int id)
        {
            Id = id;
        }
    }
}
