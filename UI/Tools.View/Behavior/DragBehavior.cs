using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Todo.UI.Tools.View.Behavior
{
    /// <summary>
    /// Behavior of drag effect initialization.
    /// </summary>
    public sealed class DragBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Attach behavior.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDown += AssociatedObjectOnMouseDown;
        }

        /// <summary>
        /// Detach behavior.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseDown -= AssociatedObjectOnMouseDown;
        }

        /// <summary>
        /// Mouse down event handler of drag effect initialization.
        /// </summary>
        /// <param name="sender">Sender of event handler. </param>
        /// <param name="e">Args of event handler. </param>
        private void AssociatedObjectOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dataObject = new DataObject(DataFormat, AssociatedObject.DataContext);
            DragDrop.DoDragDrop(AssociatedObject, dataObject, DragDropEffects.Move);
        }

        /// <summary>
        /// DataContext type of dragged control.
        /// </summary>
        public Type DataFormat
        {
            get { return (Type)GetValue(DataFormatProperty); }
            set { SetValue(DataFormatProperty, value); }
        }

        /// <summary>
        /// Dependency property of <see cref="DataFormat"/>.
        /// </summary>
        public static readonly DependencyProperty DataFormatProperty = DependencyProperty.Register(
          nameof(DataFormat), typeof(Type), typeof(DragBehavior), new PropertyMetadata(typeof(object)));
    }
}
