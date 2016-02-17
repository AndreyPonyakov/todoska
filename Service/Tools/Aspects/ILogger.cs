namespace TodoSystem.Service.Tools.Aspects
{
    /// <summary>
    /// Provides a simple logging abstraction which can be used for logging infrastructure of choice.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void WriteLine(string message);
    }
}
