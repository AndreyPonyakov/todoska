using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Base class for any viewmodel.  
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region implement INotifyPropertyChanged

        /// <summary>
        /// Propperty changed subscriber collection. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed 
        /// </summary>
        /// <param name="propertyName">Changed property name. </param>
        protected void OnPropertyChanged(string propertyName) =>
            this.OnPropertyChanged(PropertyChanged, propertyName);

        /// <summary>
        /// Notify property changed for propertry setter.
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
        /// Notify property changed for transit model property of viewmodel.
        /// </summary>
        /// <typeparam name="T">Property type. </typeparam>
        /// <param name="getValue">Property getter. </param>
        /// <param name="setValue">Property setter. </param>
        /// <param name="value">New value. </param>
        /// <param name="propertyName">Changed property name. </param>
        protected void SetField<T>(Func<T> getValue, Action<T> setValue, T value,
            [CallerMemberName] string propertyName = null)
        {
            Action notificator = () => OnPropertyChanged(propertyName);
            notificator.SetField(getValue, setValue, value);
        }

        #endregion

    }
}
