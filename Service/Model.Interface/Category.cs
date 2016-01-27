using System.Drawing;
using System.Runtime.Serialization;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Category interface.
    /// </summary>
    [DataContract]
    public class Category
    {
        /// <summary>
        /// Primary key of category.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Preferable color of category.
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        /// Priority of category.
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }
}
