using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Helper class for mix-in INotifyDataErrorInfo implementation.
    /// </summary>
    public static class NotifyDataErrorInfoHelper
    {
        /// <summary>
        /// Returns the errors for <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="errorContainer">Container with validation states. </param>
        /// <param name="propertyName">The name of the property for which the errors are requested.</param>
        /// <returns>An enumerable with the errors.</returns>
        /// <seealso cref="INotifyDataErrorInfo"/>
        public static IEnumerable GetErrors(this IDictionary<string, ICollection<string>> errorContainer, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errorContainer.ContainsKey(propertyName))
            {
                return null;
            }

            return errorContainer[propertyName];
        }

        /// <summary>
        /// Indicates whether the error status.
        /// </summary>
        /// <param name="errorContainer">Container with validation states. </param>
        /// <returns>An enumerable with the errors.</returns>
        /// <seealso cref="INotifyDataErrorInfo"/>
        public static bool HasErrors(this IDictionary<string, ICollection<string>> errorContainer)
            => errorContainer.Any();

        /// <summary>
        /// Raises the errors changed event.
        /// </summary>
        /// <typeparam name="T">Sender class. </typeparam>
        /// <param name="sender">Sender object. </param>
        /// <param name="handler">Error changed handler. </param>
        /// <param name="propertyName">The name of the changed property.</param>
        public static void OnErrorsChanged<T>(
            this T sender,
            EventHandler<DataErrorsChangedEventArgs> handler,
            string propertyName)
            => handler?.Invoke(sender, new DataErrorsChangedEventArgs(propertyName));

        /// <summary>
        /// Validates for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="errorContainer">Container with validation states. </param>
        /// <param name="notificator">Notification delegate. </param>
        /// <param name="isValid">New validate state. </param>
        /// <param name="propertyName">The name of the property. </param>
        /// <param name="message">Validate message. </param>
        /// <param name="unique">True for repeatable message. </param>
        public static void Validate(
            this IDictionary<string,
            ICollection<string>> errorContainer,
            Action<string> notificator,
            bool isValid,
            string propertyName,
            string message,
            bool unique = true)
        {
            if (isValid)
            {
                errorContainer.ClearErrors(notificator, propertyName);
            }
            else
            {
                errorContainer.AppendErrors(notificator, propertyName, message, unique);
            }
        }

        /// <summary>
        /// Append errors for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="errorContainer">Container with validation states. </param>
        /// <param name="notificator">Notification delegate. </param>
        /// <param name="propertyName">The name of the property. </param>
        /// <param name="message">Validate message. </param>
        /// <param name="unique">False for repeatable message. </param>
        public static void AppendErrors(
            this IDictionary<string,
            ICollection<string>> errorContainer,
            Action<string> notificator,
            string propertyName,
            string message,
            bool unique = true)
        {
            if (errorContainer.ContainsKey(propertyName))
            {
                if (!unique || !errorContainer[propertyName].Contains(message))
                {
                    errorContainer[propertyName].Add(message);
                    notificator(propertyName);
                }
            }
            else
            {
                errorContainer[propertyName] = new List<string> { message };
                notificator(propertyName);
            }
        }

        /// <summary>
        /// Remove errors for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="errorContainer">Container with validation states. </param>
        /// <param name="notificator">Notification delegate. </param>
        /// <param name="propertyName">The name of the property. </param>
        public static void ClearErrors(
            this IDictionary<string,
            ICollection<string>> errorContainer,
            Action<string> notificator,
            string propertyName)
        {
            if (errorContainer.ContainsKey(propertyName))
            {
                errorContainer.Remove(propertyName);
                notificator(propertyName);
            }
        }

        /// <summary>
        /// Retrieves errors status of the property.
        /// </summary>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyName">The name of the error property.</param>
        /// <returns>True if this property has errors. </returns>
        public static bool HasErrors(this INotifyDataErrorInfo sender, string propertyName)
        {
            var errors = sender.GetErrors(propertyName);
            return errors != null && errors.Cast<string>().Any();
        }

        /// <summary>
        /// Retrieves errors status of the property.
        /// </summary>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyNames">List of property name. </param>
        /// <returns>True if this property has errors. </returns>
        public static bool HasErrors(this INotifyDataErrorInfo sender, IEnumerable<string> propertyNames)
            => propertyNames.Any(p => sender.HasErrors(p));
    }
}
