using System;
using System.Security.Permissions;
using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Event
{
    /// <summary>
    /// Event argument for MoveTo event.
    /// </summary>
    /// <typeparam name="TS">Source transition data type. </typeparam>
    /// <typeparam name="TD">Destination transition data type. </typeparam>
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class MovedEventHandlerArgs<TS, TD> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovedEventHandlerArgs{TS,TD}" /> class.
        /// </summary>
        /// <param name="dataTransition">DataTransition of movement. </param>
        public MovedEventHandlerArgs(DataTransition<TS, TD> dataTransition)
        {
            DataTransition = dataTransition;
        }

        /// <summary>
        /// Gets transition information.
        /// </summary>
        public DataTransition<TS, TD> DataTransition { get; }
    }
}