using System;
using System.Windows;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Changed event handler.
    /// </summary>
    /// <param name="sender">Sender element. </param>
    /// <param name="e">Event handler arguments. </param>
    public delegate void ChangedHandler(object sender, BindingChangedEventArgs e);

    /// <summary>
    /// Binding changed argument class.
    /// </summary>
    public class BindingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingChangedEventArgs"/> class.
        /// </summary>
        /// <param name="e">Event handler arguments. </param>
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            EventArgs = e;
        }

        /// <summary>
        /// Gets nested event arguments.
        /// </summary>
        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}