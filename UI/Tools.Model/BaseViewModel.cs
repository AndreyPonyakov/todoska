using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Base class for a view model.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        /// <seealso cref="INotifyPropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event raised when the validation status changes.
        /// </summary>
        /// <seealso cref="INotifyDataErrorInfo"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value indicating whether the error status.
        /// </summary>
        /// <seealso cref="INotifyDataErrorInfo"/>
        public bool HasErrors => ErrorContainer.HasErrors();

        /// <summary>
        /// Gets container with validation states.
        /// </summary>
        protected IDictionary<string, ICollection<string>> ErrorContainer { get; } = new Dictionary<string, ICollection<string>>();

        /// <summary>
        /// Returns the errors for <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property for which the errors are requested.</param>
        /// <returns>An enumerable with the errors.</returns>
        /// <seealso cref="INotifyDataErrorInfo"/>
        public IEnumerable GetErrors(string propertyName) => ErrorContainer.GetErrors(propertyName);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Changed property name. </param>
        protected void OnPropertyChanged(string propertyName)
            => this.OnPropertyChanged(PropertyChanged, propertyName);

        /// <summary>
        /// Raises the <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnErrorsChanged(string propertyName)
            => this.OnErrorsChanged(ErrorsChanged, propertyName);

        /// <summary>
        /// Notify property changed for property setter.
        /// </summary>
        /// <typeparam name="T">Property type. </typeparam>
        /// <param name="field">Behind field name. </param>
        /// <param name="value">New value. </param>
        /// <param name="propertyName">Changed property name. </param>
        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            Action notificator = () => OnPropertyChanged(propertyName);
            notificator.SetField(ref field, value);
        }

        /// <summary>
        /// Notify property changed for transit model property of view model.
        /// </summary>
        /// <typeparam name="T">Property type. </typeparam>
        /// <param name="getValue">Property getter. </param>
        /// <param name="setValue">Property setter. </param>
        /// <param name="value">New value. </param>
        /// <param name="propertyName">Changed property name. </param>
        protected void SetField<T>(Func<T> getValue, Action<T> setValue, T value, [CallerMemberName] string propertyName = null)
        {
            Action notificator = () => OnPropertyChanged(propertyName);
            notificator.SetField(getValue, setValue, value);
        }

        /// <summary>
        /// Validates for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="isValid">New validate state. </param>
        /// <param name="propertyName">The name of the property. </param>
        /// <param name="message">Validate message. </param>
        /// <param name="unique">True for repeatable message. </param>
        protected void Validate(bool isValid, string propertyName, string message, bool unique = true)
            => ErrorContainer.Validate(OnErrorsChanged, isValid, propertyName, message, unique);

        /// <summary>
        /// Append errors for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property. </param>
        /// <param name="message">Validate message. </param>
        /// <param name="unique">False for repeatable message. </param>
        protected void AppendErrors(string propertyName, string message, bool unique = true)
            => ErrorContainer.AppendErrors(OnErrorsChanged, propertyName, message, unique);

        /// <summary>
        /// Remove errors for the property named <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property. </param>
        protected void ClearErrors(string propertyName)
            => ErrorContainer.ClearErrors(OnErrorsChanged, propertyName);
    }
}
