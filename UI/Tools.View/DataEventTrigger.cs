using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

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
                "Source",
                typeof(Binding),
                typeof(DataEventTrigger),
                new PropertyMetadata(null, HandleSourceChanged));

        /// <summary>
        /// Backing DP for the EventName property
        /// </summary>
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register(
                "EventName",
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
            _listener = new BindingListener(HandleBindingValueChanged);
            _listener.Binding = new Binding();
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

            this._listener.Element = this.AssociatedObject;
        }

        /// <summary>
        /// Detaches behavior.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this._listener.Element = null;
        }

        /// <summary>
        /// Notification that the Source has changed.
        /// </summary>
        /// <param name="e">Dependency object changed arguments. </param>
        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            this._listener.Binding = this.Source;
        }

        /// <summary>
        /// Notification that the EventName has changed.
        /// </summary>
        /// <param name="e">Dependency object changed arguments. </param>
        protected virtual void OnEventNameChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateHandler();
        }

        private static void HandleEventNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DataEventTrigger)sender).OnEventNameChanged(e);
        }

        private static void HandleSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DataEventTrigger)sender).OnSourceChanged(e);
        }

        private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
            this.UpdateHandler();
        }

        private void UpdateHandler()
        {
            if (_currentEvent != null)
            {
                _currentEvent.RemoveEventHandler(this._currentTarget, this._currentDelegate);

                _currentEvent = null;
                _currentTarget = null;
                _currentDelegate = null;
            }

            this._currentTarget = this._listener.Value;

            if (this._currentTarget != null && !string.IsNullOrEmpty(this.EventName))
            {
                Type targetType = this._currentTarget.GetType();
                this._currentEvent = targetType.GetEvent(this.EventName);
                if (this._currentEvent != null)
                {
                    var handlerMethod = this.GetType().GetMethod("OnEvent", BindingFlags.NonPublic | BindingFlags.Instance);
                    this._currentDelegate = this.GetDelegate(this._currentEvent, this.OnMethod);
                    this._currentEvent.AddEventHandler(this._currentTarget, this._currentDelegate);
                }
            }
        }

        private Delegate GetDelegate(EventInfo eventInfo, Action action)
        {
            if (typeof(EventHandler).IsAssignableFrom(eventInfo.EventHandlerType))
            {
                MethodInfo method = this.GetType().GetMethod("OnEvent", BindingFlags.NonPublic | BindingFlags.Instance);
                return Delegate.CreateDelegate(eventInfo.EventHandlerType, this, method);
            }

            Type handlerType = eventInfo.EventHandlerType;
            ParameterInfo[] eventParams = handlerType.GetMethod("Invoke").GetParameters();

            IEnumerable<ParameterExpression> parameters = eventParams.Select(p => System.Linq.Expressions.Expression.Parameter(p.ParameterType, "x"));

            MethodCallExpression methodExpression = System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(action), action.GetType().GetMethod("Invoke"));
            LambdaExpression lambdaExpression = System.Linq.Expressions.Expression.Lambda(methodExpression, parameters.ToArray());
            return Delegate.CreateDelegate(handlerType, lambdaExpression.Compile(), "Invoke", false);
        }

        private void OnMethod()
        {
            this.InvokeActions(null);
        }

        private void OnEvent(object sender, EventArgs e)
        {
            this.InvokeActions(e);
        }
    }
}
