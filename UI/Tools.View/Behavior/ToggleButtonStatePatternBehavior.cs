using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using Todo.UI.Tools.View.Converter;

namespace Todo.UI.Tools.View.Behavior
{
    /// <summary>
    /// Behavior for implement of view change between state.
    /// </summary>
    public class ToggleButtonStatePatternBehavior : Behavior<ToggleButton>
    {

        /// <summary>
        /// Attaching
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += AssociatedObjectOnChecked;

            var binding = new MultiBinding { Converter = new StatePatternToBooleanConverter() };

            binding.Bindings.Add(new Binding(nameof(State)) { Source = this });
            binding.Bindings.Add(new Binding(nameof(TargetState)) { Source = this });

            AssociatedObject.SetBinding(ToggleButton.IsCheckedProperty, binding);
        }

        /// <summary>
        /// Attached event handler.
        /// </summary>
        /// <param name="sender">Sender. </param>
        /// <param name="e">Event argument. </param>
        private void AssociatedObjectOnChecked(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.IsChecked == true)
            {
                if (State != null && TargetState != null &&
                    State != TargetState)
                {
                    State = TargetState;
                }
            }

        }

        /// <summary>
        /// Current state of VM.
        /// </summary>
        public object State
        {
            get { return GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
          nameof(State), typeof(object), typeof(ToggleButtonStatePatternBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Target state of VM when V must show.
        /// </summary>
        public object TargetState
        {
            get { return GetValue(TargetStateProperty); }
            set { SetValue(TargetStateProperty, value); }
        }

        public static readonly DependencyProperty TargetStateProperty = DependencyProperty.Register(
          nameof(TargetState), typeof(object), typeof(ToggleButtonStatePatternBehavior), new PropertyMetadata(null));

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click += AssociatedObjectOnChecked;
        }
    }
}
