using System;

using TodoSystem.Service.Tools.Aspects;

namespace TodoSystem.Service.Host
{
    /// <summary>
    /// Provides a simple logging infrastructure.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Logs a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
