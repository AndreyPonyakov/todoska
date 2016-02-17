using System;
using System.Runtime.Serialization;
using System.Security;

namespace TodoSystem.Service.Model.Interface.Exceptions
{
    /// <summary>
    /// Exception class of foreign key reference consistency.
    /// </summary>
    [Serializable]
    public class ForeignKeyConstraintException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraintException"/> class.
        /// </summary>
        /// <param name="foreignKey">External table with foreign key constraint. </param>
        public ForeignKeyConstraintException(string foreignKey)
            : base($"Operation cannot continue: foreign key constraint in {foreignKey}.")
        {
            ForeignKey = foreignKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraintException"/> class.
        /// message and inner exception.
        /// </summary>
        /// <param name="message">Error message text.</param>
        /// <param name="innerException">The original (inner) exception.</param>
        public ForeignKeyConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraintException"/> class.
        /// </summary>
        /// <param name="info">Holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">Contains contextual information about the source or destination. </param>
        protected ForeignKeyConstraintException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets external table with foreign key constraint.
        /// </summary>
        public string ForeignKey { get; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ForeignKey), ForeignKey);
            base.GetObjectData(info, context);
        }
    }
}
