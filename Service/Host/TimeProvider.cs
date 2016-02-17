using System;

using TodoSystem.Service.Tools.Aspects;

namespace TodoSystem.Service.Host
{
    /// <summary>
    /// Class which provides current time using <see cref="DateTime.Now"/>.
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
        /// <summary>
        /// Gets current time.
        /// </summary>
        /// <returns>Current time. </returns>
        public DateTime GetNow() => DateTime.Now;
    }
}
