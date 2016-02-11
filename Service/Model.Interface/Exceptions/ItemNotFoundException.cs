using System;
using System.Runtime.Serialization;

namespace TodoSystem.Service.Model.Interface.Exceptions
{
    /// <summary>
    /// Exception class of record's absence.
    /// </summary>
    [Serializable]
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        public ItemNotFoundException()
            : base("Operation cannot continue: current item didn't find.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// message and inner exception.
        /// </summary>
        /// <param name="message">Error message text.</param>
        /// <param name="innerException">The original (inner) exception.</param>
        public ItemNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="info">Holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">Contains contextual information about the source or destination. </param>
        protected ItemNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
