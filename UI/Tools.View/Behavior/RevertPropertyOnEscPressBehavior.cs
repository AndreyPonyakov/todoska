using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TodoSystem.UI.Tools.View.Behavior
{
    /// <summary>
    /// Behavior class for revert changing property on enter press.
    /// </summary>
    public class RevertPropertyOnEscPressBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// Target element.
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register(
                nameof(Element),
                typeof(DependencyObject),
                typeof(RevertPropertyOnEscPressBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Target dependency property.
        /// </summary>
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register(
                nameof(Property),
                typeof(DependencyProperty),
                typeof(RevertPropertyOnEscPressBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property of <see cref="Command"/>.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(RevertPropertyOnEscPressBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets target element.
        /// </summary>
        public DependencyObject Element
        {
            get { return (DependencyObject)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        /// <summary>
        /// Gets or sets target dependency property.
        /// </summary>
        public DependencyProperty Property
        {
            get { return (DependencyProperty)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        /// <summary>
        /// Gets or sets command of revert changes.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Attach behavior.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObjectOnPreviewKeyDown;
        }

        /// <summary>
        /// Detach behavior.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObjectOnPreviewKeyDown;
        }

        /// <summary>
        /// Attaching event handler.
        /// </summary>
        /// <param name="sender">Sender control. </param>
        /// <param name="e">Argument container. </param>
        private void AssociatedObjectOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (Element != null && Property != null)
                {
                    BindingOperations.GetBindingExpression(Element, Property)?.UpdateTarget();
                    BindingOperations.GetMultiBindingExpression(Element, Property)?.UpdateTarget();
                    if (Command != null && Command.CanExecute(null))
                    {
                        Command.Execute(null);
                    }
                }
            }
        }
    }
}
