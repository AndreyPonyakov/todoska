using System;
using System.Security.Permissions;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel.Event
{
    /// <summary>
    /// MoveTo event handler.
    /// </summary>
    /// <typeparam name="TS">Source transition data type. </typeparam>
    /// <typeparam name="TD">Destination transition data type. </typeparam>
    /// <param name="sender">Sender element. </param>
    /// <param name="args">Event handler arguments. </param>
    public delegate void MoveToEventHandler<TS, TD>(object sender, MoveToEventHandlerArgs<TS,TD> args);

    /// <summary>
    /// Event argument for MoveTo event.
    /// </summary>
    /// <typeparam name="TS">Source transition data type. </typeparam>
    /// <typeparam name="TD">Destination transition data type. </typeparam>
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class MoveToEventHandlerArgs<TS, TD> : EventArgs
    {

        public DataTransition<TS,TD> DataTransition { get; }

        /// <summary>
        /// Create <see cref="MoveToEventHandler{TD,TS}"/> instance.
        /// </summary>
        /// <param name="dataTransition">DataTransition of movement. </param>
        public MoveToEventHandlerArgs(DataTransition<TS, TD> dataTransition)
        {
            DataTransition = dataTransition;
        }
    }
}
