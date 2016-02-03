using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Class for create property changed dependency.
    /// </summary>
    public static class NotifyPropertyChangeSubscriber
    {
        /// <summary>
        /// Create dependency from INPC property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyName">Property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetPropertyChanged<T>(this T sender, string propertyName, Action handler)
            where T : class, INotifyPropertyChanged
        {
            if (sender == null)
            {
                return null;
            }

            sender.PropertyChanged +=
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
        /// Create dependency from INPC property with once execute.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyName">Property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetPropertyChangedWithExecute<T>(this T sender, string propertyName, Action handler)
            where T : class, INotifyPropertyChanged
        {
            handler();
            return sender.SetPropertyChanged(propertyName, handler);
        }

        /// <summary>
        /// Create dependency from INPC property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyNames">List of property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetPropertyChanged<T>(this T sender, IEnumerable<string> propertyNames, Action handler)
            where T : class, INotifyPropertyChanged
        {
            return sender.SetPropertyChanged(propertyNames, propertyName => handler());
        }

        /// <summary>
        /// Create dependency from INPC property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="propertyNames">List of property name. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        private static T SetPropertyChanged<T>(this T sender, IEnumerable<string> propertyNames, Action<string> handler)
            where T : class, INotifyPropertyChanged
        {
            if (propertyNames != null && sender != null)
            {
                sender.PropertyChanged +=
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
