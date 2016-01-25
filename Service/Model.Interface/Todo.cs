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
        /// Primary key.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Short title text.
        /// </summary>
        [DataMember]
        public string Title {  get; set; }

        /// <summary>
        /// Long text body.
        /// </summary>
        [DataMember]
        public string Desc {  get; set; }

        /// <summary>
        /// Deadline date.
        /// </summary>
        [DataMember]
        public DateTime Deadline {get; set; }

        /// <summary>
        /// Primary key of compounded category.
        /// </summary>
        [DataMember]
        public int CategoryId {  get; set; }

        /// <summary>
        /// Checked state.
        /// </summary>
        [DataMember]
        public bool Checked { get; set; }

        /// <summary>
        /// Priority.
        /// </summary>
        [DataMember]
        public int Order {  get; set; }

    }
}
