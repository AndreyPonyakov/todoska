using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Class-aggregator to store error message.
    /// </summary>
    public class FaultExceptionManager : IExceptionManager
    {
        private readonly IDictionary<Type, string> _container = new Dictionary<Type, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultExceptionManager"/> class.
        /// </summary>
        /// <param name="defaultMessage">Default error message. </param>
        public FaultExceptionManager(string defaultMessage)
        {
            DefaultMessage = defaultMessage;
        }

        /// <summary>
        /// Gets default error message.
        /// </summary>
        public string DefaultMessage { get; }

        /// <summary>
        /// Registers a new exception type with a message.
        /// </summary>
        /// <typeparam name="T">Exception type. </typeparam>
        /// <param name="message">Exception message. </param>
        /// <returns>Current <see cref="FaultExceptionManager"/> to the fluent syntax. </returns>
        public FaultExceptionManager Register<T>(string message)
            where T : Exception
        {
            _container[typeof(T)] = message;
            return this;
        }

        /// <summary>
        /// Resolves a caught exception.
        /// </summary>
        /// <param name="exception">A received exception. </param>
        /// <returns>An appropriate message. </returns>
        public string Resolve(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var exeptionType = exception.GetType();

            if (_container.ContainsKey(exeptionType))
            {
                return _container[exeptionType];
            }

            var key = _container.Keys.Aggregate<Type, Type>(
                null,
                (result, current) =>
                {
                    if (current == exeptionType || current.IsSubclassOf(exeptionType))
                    {
                        if (result == null || current.IsSubclassOf(result))
                        {
                            return current;
                        }
                    }

                    return result;
                });
            return key == null ? DefaultMessage : _container[key];
        }
    }
}