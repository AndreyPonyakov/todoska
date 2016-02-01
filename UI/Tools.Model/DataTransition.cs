using System.Diagnostics.CodeAnalysis;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Data transition struct for transition operation.
    /// </summary>
    /// <typeparam name="TS">Source data type. </typeparam>
    /// <typeparam name="TD">Destination data type. </typeparam>
    public class DataTransition<TS, TD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransition{TS,TD}"/> class. 
        /// </summary>
        /// <param name="source">Source data of operation. </param>
        /// <param name="destination">Destination data of operation. </param>
        public DataTransition(TS source, TD destination)
        {
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// Gets source data of operation.
        /// </summary>
        public TS Source { get; }

        /// <summary>
        /// Gets destination data of operation.
        /// </summary>
        public TD Destination { get; }
    }

    /// <summary>
    /// Data transition struct for transition operation.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Template class.")]
    public class DataTransition : DataTransition<object, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransition"/> class. 
        /// </summary>
        /// <param name="source">Source data of operation. </param>
        /// <param name="destination">Destination data of operation. </param>
        public DataTransition(object source, object destination)
            : base(source, destination)
        {
        }

        /// <summary>
        /// Convert to specialization Data transition struct.
        /// </summary>
        /// <typeparam name="TS">Source data type. </typeparam>
        /// <typeparam name="TD">Destination data type. </typeparam>
        /// <returns><see cref="DataTransition{TS,TD}"/> instance. </returns>
        public DataTransition<TS, TD> Cast<TS, TD>()
        {
            return new DataTransition<TS, TD>((TS)Source, (TD)Destination);
        }
    }
}
