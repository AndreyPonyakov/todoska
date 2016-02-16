using System;
using System.Runtime.Serialization;

namespace TodoSystem.Service.Model.Interface.Exceptions
{
    /// <summary>
    /// Exception class of validation error.
    /// </summary>
    [Serializable]
    public class DataValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataValidationException"/> class.
        /// </summary>
        public DataValidationException()
            : base("Operation cannot continue: content did not pass validation.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataValidationException"/> class.
        /// message and inner exception.
        /// </summary>
        /// <param name="message">Error message text.</param>
        /// <param name="innerException">The original (inner) exception.</param>
        public DataValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataValidationException"/> class.
        /// </summary>
        /// <param name="info">Holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">Contains contextual information about the source or destination. </param>
        protected DataValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
