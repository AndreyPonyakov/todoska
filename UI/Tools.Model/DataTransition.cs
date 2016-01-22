namespace Todo.UI.Tools.Model
{
    /// <summary>
    /// Data transition struct for drag'n'drop operation.
    /// </summary>
    /// <typeparam name="TS">Source data type. </typeparam>
    /// <typeparam name="TD">Destination data type. </typeparam>
    public class DataTransition<TS, TD>
    {
        /// <summary>
        /// Source data of operation.
        /// </summary>
        public TS Source { get; }

        /// <summary>
        /// Destination data of operation.
        /// </summary>
        public TD Destination { get; }

        /// <summary>
        /// Create <see cref="DataTransition{TS,TD}"/> instance. 
        /// </summary>
        /// <param name="source">Source data of operation. </param>
        /// <param name="destination">Destination data of operation. </param>
        public DataTransition(TS source, TD destination)
        {
            Source = source;
            Destination = destination;
        }
    }

    /// <summary>
    /// Data transition struct for drag'n'drop operation.
    /// </summary>
    public class DataTransition : DataTransition<object, object>
    {
        /// <summary>
        /// Create <see cref="DataTransition"/> instance. 
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
