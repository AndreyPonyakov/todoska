using System.Runtime.Serialization;

namespace TodoSystem.Service.Model.Interface.Faults
{
    /// <summary>
    /// Fault class of foreign key reference consistency.
    /// </summary>
    [DataContract]
    public class ForeignKeyConstraintFault
    {
        /// <summary>
        /// Gets or sets external table with foreign key constraint.
        /// </summary>
        [DataMember]
        public string ForeignKey { get; set; }
    }
}
