using System;
using System.Drawing;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    /// <summary>
    /// Fake class of ICategory
    /// </summary>
    public class FakeCategory : ICategory
    {
        public int Id { get; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Create instance of <see cref="FakeCategory"/>
        /// </summary>
        /// <param name="id"></param>
        public FakeCategory(int id)
        {
            Id = id;
        }
    }
}
