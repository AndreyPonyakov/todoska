using System;
using System.ComponentModel;

namespace Todo.UI.Tools.Model
{
    /// <summary>
    /// Class for create dependency.
    /// </summary>
    public static class NotifySubscribe
    {
        /// <summary>
        /// Create dependency from INPC property.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <param name="propertyName">Property name. </param>
        /// <returns>Sender class for fluent syntax. </returns>
        public static T SetPropertyChanged<T>(this T sender,
            string propertyName, Action handler)
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
    }
}
