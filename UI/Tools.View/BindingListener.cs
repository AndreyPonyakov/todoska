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
            get
            {
                return _binding;
            }

            set
            {
                _binding = value;
                Attach();
            }
        }

        /// <summary>
        /// Gets or sets binding element.
        /// </summary>
        public FrameworkElement Element
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
                Attach();
            }
        }

        /// <summary>
        /// Gets binding value.
        /// </summary>
        public object Value { get; private set; }

        private static List<DependencyPropertyListener> FreeListeners { get; } = new List<DependencyPropertyListener>();

        private void Attach()
        {
            Detach();

            if (_target != null && _binding != null)
            {
                _listener = GetListener();
                _listener.Attach(_target, _binding);
            }
        }

        private void Detach()
        {
            if (_listener != null)
            {
                ReturnListener();
            }
        }

        private DependencyPropertyListener GetListener()
        {
            DependencyPropertyListener listener;

            if (FreeListeners.Count != 0)
            {
                listener = FreeListeners[FreeListeners.Count - 1];
                FreeListeners.RemoveAt(FreeListeners.Count - 1);
            }
            else
            {
                listener = new DependencyPropertyListener();
                listener.Changed += HandleValueChanged;
            }

            return listener;
        }

        private void ReturnListener()
        {
            _listener.Changed -= HandleValueChanged;
            FreeListeners.Add(_listener);
            _listener = null;
        }

        private void HandleValueChanged(object sender, BindingChangedEventArgs e)
        {
            Value = e.EventArgs.NewValue;
            _changedHandler(this, e);
        }
    }
}
