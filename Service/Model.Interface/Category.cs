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
        /// Gets or sets primary key of category.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets preferable color of category.
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets priority of category in list.
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }
}
