using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Todo.UI.Tools.Model
{
    /// <summary>
    /// Helper class for implement INotifyPropertyChanged.
    /// </summary>
    public static class NotifyPropertyChangedHelper
    {
        /// <summary>
        /// Notify property changed.
        /// </summary>
        /// <typeparam name="T">Sender class type. </typeparam>
        /// <param name="sender">Sender class. </param>
        /// <param name="handler">Property changed handler. </param>
        /// <param name="propertyName">Property name. </param>
        public static void OnPropertyChanged<T>(this T sender,
            PropertyChangedEventHandler handler, string propertyName) where T : class, INotifyPropertyChanged
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notify property changed for transit model property of viewmodel.
        /// </summary>
        /// <typeparam name="T">Property type. </typeparam>
        /// <param name="notificator">Notification delegate. </param>
        /// <param name="getValue">Property getter. </param>
        /// <param name="setValue">Property setter. </param>
        /// <param name="value">New value. </param>
        /// <returns>True if property changed. </returns>
        public static bool SetField<T>(this Action notificator, Func<T> getValue, Action<T> setValue, T value)
        {
            var oldValue = getValue();
            var changed = !EqualityComparer<T>.Default.Equals(oldValue, value);

            if (changed)
            {
                setValue(value);
                notificator();
            }

            return changed;
        }

        /// <summary>
        /// Notify property changed for propertry setter.
        /// </summary>
        /// <typeparam name="T">Property type. </typeparam>
        /// <param name="notificator">Notification delegate. </param>
        /// <param name="field">Behind field name. </param>
        /// <param name="value">New value. </param>
        /// <returns>True if property changed. </returns>
        public static bool SetField<T>(this Action notificator, ref T field, T value)
        {
            var oldValue = field;
            var changed = !EqualityComparer<T>.Default.Equals(oldValue, value);

            if (changed)
            {
                field = value;
                notificator();
            }

            return changed;
        }

    }
}
