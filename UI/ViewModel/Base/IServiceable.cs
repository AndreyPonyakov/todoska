using System.ComponentModel;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Interface for view model with service interaction.
    /// </summary>
    /// <typeparam name="T">Service type. </typeparam>
    public interface IServiceable<out T> : INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets back-end service.
        /// </summary>
        T Service { get; }
    }
}
