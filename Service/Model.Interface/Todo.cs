using System;
using System.Runtime.Serialization;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// DTO class of Todo.
    /// </summary>
    [DataContract]
    public class Todo
    {
        /// <summary>
        /// Gets or sets a value of primary key.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets short text title.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets long text body.
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// Gets or sets deadline date.
        /// </summary>
        [DataMember]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Gets or sets primary key of attached category.
        /// </summary>
        [DataMember]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checked state.
        /// </summary>
        [DataMember]
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }
}
