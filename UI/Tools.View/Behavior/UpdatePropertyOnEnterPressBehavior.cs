using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TodoSystem.UI.Tools.View.Behavior
{
    /// <summary>
    /// Behavior class for update property on enter press.
    /// </summary>
    public class UpdatePropertyOnEnterPressBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// Target element.
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register(
                nameof(Element),
                typeof(DependencyObject),
                typeof(UpdatePropertyOnEnterPressBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Target dependency property.
        /// </summary>
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register(
                nameof(Property),
                typeof(DependencyProperty),
                typeof(UpdatePropertyOnEnterPressBehavior),
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
            if (e.Key != Key.Return)
            {
                return;
            }

            if (Element == null || Property == null)
            {
                return;
            }

            BindingOperations.GetBindingExpression(Element, Property)?.UpdateSource();
            BindingOperations.GetMultiBindingExpression(Element, Property)?.UpdateSource();
        }
    }
}
