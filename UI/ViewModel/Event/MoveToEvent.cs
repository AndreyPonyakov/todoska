using System;
using System.Security.Permissions;
using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Event
{
    /// <summary>
    /// MoveTo event handler.
    /// </summary>
    /// <typeparam name="TS">Source transition data type. </typeparam>
    /// <typeparam name="TD">Destination transition data type. </typeparam>
    /// <param name="sender">Sender element. </param>
    /// <param name="args">Event handler arguments. </param>
    public delegate void MoveToEventHandler<TS, TD>(object sender, MoveToEventHandlerArgs<TS, TD> args);

    /// <summary>
    /// Event argument for MoveTo event.
    /// </summary>
    /// <typeparam name="TS">Source transition data type. </typeparam>
    /// <typeparam name="TD">Destination transition data type. </typeparam>
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class MoveToEventHandlerArgs<TS, TD> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToEventHandlerArgs{TS,TD}" /> class.
        /// </summary>
        /// <param name="dataTransition">DataTransition of movement. </param>
        public MoveToEventHandlerArgs(DataTransition<TS, TD> dataTransition)
        {
            DataTransition = dataTransition;
        }

        /// <summary>
        /// Gets transition information.
        /// </summary>
        public DataTransition<TS, TD> DataTransition { get; }
    }
}
