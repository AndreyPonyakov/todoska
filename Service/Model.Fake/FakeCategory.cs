using System;
using System.Drawing;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
<<<<<<< HEAD
=======
    /// <summary>
    /// Fake class of ICategory
    /// </summary>
>>>>>>> 3c9c713914bbb74969f1190d766c6af8ccc07297
    public class FakeCategory : ICategory
    {
        public int Id { get; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Order { get; set; }

<<<<<<< HEAD
=======
        /// <summary>
        /// Create instance of <see cref="FakeCategory"/>
        /// </summary>
        /// <param name="id"></param>
>>>>>>> 3c9c713914bbb74969f1190d766c6af8ccc07297
        public FakeCategory(int id)
        {
            Id = id;
        }
    }
}
