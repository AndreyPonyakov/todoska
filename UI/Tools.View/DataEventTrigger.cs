using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Expression = System.Linq.Expressions.Expression;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Trigger which fires when a CLR event is raised on an object.
    /// Can be used to trigger from events on the data context, as opposed to
    /// a standard EventTrigger which uses routed events on FrameworkElements.
    /// </summary>
    public class DataEventTrigger : TriggerBase<FrameworkElement>
    {
        /// <summary>
        /// Backing DP for the Source property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(Binding),
                typeof(DataEventTrigger),
                new PropertyMetadata(null, HandleSourceChanged));

        /// <summary>
        /// Backing DP for the EventName property
        /// </summary>
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register(
                nameof(EventName),
                typeof(string),
                typeof(DataEventTrigger),
                new PropertyMetadata(null, HandleEventNameChanged));

        private readonly BindingListener _listener;
        private EventInfo _currentEvent;
        private Delegate _currentDelegate;
        private object _currentTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEventTrigger"/> class.
        /// </summary>
        public DataEventTrigger()
        {
            _listener = new BindingListener(HandleBindingValueChanged) { Binding = new Binding() };
        }

        /// <summary>
        /// Gets or sets the source object for the event
        /// </summary>
        public Binding Source
        {
            get { return (Binding)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the event which triggers this
        /// </summary>
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        /// <summary>
        /// Attaches behavior.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            _listener.Element = AssociatedObject;
        }

        /// <summary>
        /// Detaches behavior.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            _listener.Element = null;
        }

        /// <summary>
        /// Notification that the Source has changed.
        /// </summary>
        /// <param name="e">Dependency object changed arguments. </param>
        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            _listener.Binding = Source;
        }

        /// <summary>
        /// Notification that the EventName has changed.
        /// </summary>
        /// <param name="e">Dependency object changed arguments. </param>
        protected virtual void OnEventNameChanged(DependencyPropertyChangedEventArgs e) => UpdateHandler();

        private static void HandleEventNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DataEventTrigger)sender).OnEventNameChanged(e);
        }

        private static void HandleSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DataEventTrigger)sender).OnSourceChanged(e);
        }

        private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e) => UpdateHandler();

        private void UpdateHandler()
        {
            if (_currentEvent != null)
            {
                _currentEvent.RemoveEventHandler(_currentTarget, _currentDelegate);
                _currentEvent = null;
                _currentTarget = null;
                _currentDelegate = null;
            }

            _currentTarget = _listener.Value;

            if (_currentTarget != null && !string.IsNullOrEmpty(EventName))
            {
                var targetType = _currentTarget.GetType();
                _currentEvent = targetType.GetEvent(EventName);
                if (_currentEvent != null)
                {
                    _currentDelegate = GetDelegate(_currentEvent, OnMethod);
                    _currentEvent.AddEventHandler(_currentTarget, _currentDelegate);
                }
            }
        }

        private Delegate GetDelegate(EventInfo eventInfo, Action action)
        {
            if (typeof(EventHandler).IsAssignableFrom(eventInfo.EventHandlerType))
            {
                var method = GetType().GetMethod(nameof(OnEvent), BindingFlags.NonPublic | BindingFlags.Instance);
                return Delegate.CreateDelegate(eventInfo.EventHandlerType, this, method);
            }

            var handlerType = eventInfo.EventHandlerType;
            var eventParams =
                handlerType.GetMethod(nameof(action.Invoke)).GetParameters();

            var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, "x"));

            var methodExpression = Expression.Call(
                Expression.Constant(action),
                action.GetType().GetMethod(nameof(action.Invoke)));
            var lambdaExpression = Expression.Lambda(methodExpression, parameters.ToArray());
            return Delegate.CreateDelegate(
                handlerType,
                lambdaExpression.Compile(),
                nameof(action.Invoke),
                false);
        }

        private void OnMethod() => InvokeActions(null);

        private void OnEvent(EventArgs e) => InvokeActions(e);
    }
}
