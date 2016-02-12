using System;

namespace TodoSystem.Service.Tools.Aspects
{
    /// <summary>
    /// Interface which provides current time.
    /// </summary>
    /// <remarks><see cref="DateTime.Now"/> is not determined function.</remarks>
    public interface ITimeProvider
    {
        /// <summary>
        /// Gets current time.
        /// </summary>
        /// <returns>Current time. </returns>
        DateTime GetNow();
    }
}
