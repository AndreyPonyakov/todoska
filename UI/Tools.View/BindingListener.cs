using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Event listener binder class.
    /// </summary>
    public class BindingListener
    {
        private readonly ChangedHandler _changedHandler;
        private DependencyPropertyListener _listener;
        private Binding _binding;
        private FrameworkElement _target;
        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingListener"/> class.
        /// </summary>
        /// <param name="changedHandler">Changed event handler. </param>
        public BindingListener(ChangedHandler changedHandler)
        {
            _changedHandler = changedHandler;
        }

        /// <summary>
        /// Gets or sets binding object.
        /// </summary>
        public Binding Binding
        {
            get { return this._binding; }
            set
            {
                this._binding = value;
                this.Attach();
            }
        }

        /// <summary>
        /// Gets or sets binding element.
        /// </summary>
        public FrameworkElement Element
        {
            get { return this._target; }
            set
            {
                this._target = value;
                this.Attach();
            }
        }

        /// <summary>
        /// Gets binding value.
        /// </summary>
        public object Value
        {
            get { return this._value; }
        }

        private static List<DependencyPropertyListener> FreeListeners { get; } = new List<DependencyPropertyListener>();

        private void Attach()
        {
            this.Detach();

            if (this._target != null && this._binding != null)
            {
                this._listener = this.GetListener();
                this._listener.Attach(_target, _binding);
            }
        }

        private void Detach()
        {
            if (this._listener != null)
            {
                this.ReturnListener();
            }
        }

        private DependencyPropertyListener GetListener()
        {
            DependencyPropertyListener listener;

            if (FreeListeners.Count != 0)
            {
                listener = FreeListeners[FreeListeners.Count - 1];
                FreeListeners.RemoveAt(FreeListeners.Count - 1);
                return listener;
            }
            else
            {
                listener = new DependencyPropertyListener();
            }

            listener.Changed += HandleValueChanged;
            return listener;
        }

        private void ReturnListener()
        {
            this._listener.Changed -= this.HandleValueChanged;
            FreeListeners.Add(this._listener);
            this._listener = null;
        }

        private void HandleValueChanged(object sender, BindingChangedEventArgs e)
        {
            this._value = e.EventArgs.NewValue;
            this._changedHandler(this, e);
        }
    }
}
