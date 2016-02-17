using System;

namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Abstract manager to resolve server fault message.
    /// </summary>
    public interface IFaultExceptionManager
    {
        /// <summary>
        /// Resolves a caught exception.
        /// </summary>
        /// <param name="exception">A received exception. </param>
        /// <returns>The appropriate message. </returns>
        string Resolve(Exception exception);
    }
}