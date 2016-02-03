using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Class for create data error info dependency.
    /// </summary>
    public static class NotifyDataErrorInfoSubscriber
    {
        /// <summary>
        /// Create dependency from INDEI property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyName">Property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetDataErrorInfo<T>(this T sender, string propertyName, Action handler)
            where T : class, INotifyDataErrorInfo
        {
            if (sender == null)
            {
                return null;
            }

            sender.ErrorsChanged +=
                (o, args) =>
                {
                    if (args.PropertyName == propertyName)
                    {
                        handler();
                    }
                };

            return sender;
        }

        /// <summary>
        /// Create dependency from INDEI property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyNames">List of property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetDataErrorInfo<T>(this T sender, IEnumerable<string> propertyNames, Action handler)
            where T : class, INotifyDataErrorInfo
        {
            return sender.SetDataErrorInfo(propertyNames, propertyName => handler());
        }

        /// <summary>
        /// Create dependency from INDEI property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyNames">List of property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        private static T SetDataErrorInfo<T>(this T sender, IEnumerable<string> propertyNames, Action<string> handler)
            where T : class, INotifyDataErrorInfo
        {
            if (propertyNames != null && sender != null)
            {
                sender.ErrorsChanged +=
                    (o, args) =>
                    {
                        if (propertyNames.Contains(args.PropertyName))
                        {
                            handler(args.PropertyName);
                        }
                    };
            }

            return sender;
        }
    }
}
