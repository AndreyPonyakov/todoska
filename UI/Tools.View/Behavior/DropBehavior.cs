using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.Tools.View.Behavior
{
    /// <summary>
    /// Behavior of drop effect initialization.
    /// </summary>
    public sealed  class DropBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Attach behavior.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Drop += AssociatedObjectOnDrop;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
        }

        /// <summary>
        /// Detach behavior.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Drop -= AssociatedObjectOnDrop;
            AssociatedObject.DragEnter -= AssociatedObjectOnDragEnter;
        }

        /// <summary>
        /// Drag enter handler of drop effect initialization.
        /// </summary>
        /// <param name="sender">Sender of event handler. </param>
        /// <param name="e">Args of event handler. </param>
        private void AssociatedObjectOnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormat))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Drop handler of drop effect initialization.
        /// </summary>
        /// <param name="sender">Sender of event handler. </param>
        /// <param name="e">Args of event handler. </param>
        private void AssociatedObjectOnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormat))
            {
                return;
            }

            var param = new DataTransition(
                e.Data.GetData(DataFormat),
                AssociatedObject.DataContext);

            Command?.Execute(param);
        }

        /// <summary>
        /// DataContext type of drop accepted control.
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
          nameof(DataFormat), typeof(Type), typeof(DropBehavior), new PropertyMetadata(typeof(object)));

        /// <summary>
        /// Command of finish drag'n'drop action.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Dependency property of <see cref="Command"/>.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
          nameof(Command), typeof(ICommand), typeof(DropBehavior), new PropertyMetadata(null));

    }
}
