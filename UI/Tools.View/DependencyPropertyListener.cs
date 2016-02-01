using System;
using System.Windows;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Class for the listening dependency property owner class.
    /// </summary>
    public sealed class DependencyPropertyListener
    {
        private static int _index;
        private readonly DependencyProperty _property;
        private FrameworkElement _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyListener"/> class.
        /// </summary>
        public DependencyPropertyListener()
        {
            _property = DependencyProperty.RegisterAttached(
                nameof(DependencyPropertyListener) + _index++, 
                typeof(object), 
                typeof(DependencyPropertyListener), 
                new PropertyMetadata(null, HandleValueChanged));
        }

        /// <summary>
        /// Changed property event handler.
        /// </summary>
        public event EventHandler<BindingChangedEventArgs> Changed;

        /// <summary>
        /// Attach behavior.
        /// </summary>
        /// <param name="element">Attaching element. </param>
        /// <param name="binding">Attaching binding. </param>
        public void Attach(FrameworkElement element, Binding binding)
        {
            if (_target != null)
            {
                throw new Exception("Cannot attach an already attached listener");
            }

            _target = element;
            _target.SetBinding(_property, binding);
        }

        /// <summary>
        /// Detach behavior.
        /// </summary>
        public void Detach()
        {
            _target.ClearValue(_property);
            _target = null;
        }

        private void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Changed?.Invoke(this, new BindingChangedEventArgs(e));
        }
    }
}