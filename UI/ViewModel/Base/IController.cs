using System.ComponentModel;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Interface for controller view model .
    /// </summary>
    /// <typeparam name="T">Service type. </typeparam>
    public interface IController<out T> : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets back-end service.
        /// </summary>
        T Service { get; }
    }
}
