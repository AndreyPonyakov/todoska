using Microsoft.Practices.Unity.InterceptionExtension;

namespace TodoSystem.Service.Tools.Aspects
{
    /// <summary>
    /// Loggable interceptor.
    /// </summary>
    public class LoggableCallHandler : ICallHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableCallHandler"/> class.
        /// </summary>
        /// <param name="logger">Logger instance. </param>
        /// <param name="timeProvider">Time provider instance. </param>
        public LoggableCallHandler(ILogger logger, ITimeProvider timeProvider)
        {
            Logger = logger;
            TimeProvider = timeProvider;
        }

        /// <summary>
        /// Gets current time provider.
        /// </summary>
        public ITimeProvider TimeProvider { get; }

        /// <summary>
        /// Gets current logger.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets or sets order in which the handler will be executed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Implement this method to execute your handler processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target. </param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the handler chain. </param>
        /// <returns>Return value from the target. </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var result = getNext()(input, getNext);
            Logger.WriteLine($"[{TimeProvider.GetNow().ToString("yyyy/MM/dd hh:mm:ss")}]");
            Logger.WriteLine($"Call: {input.Target.GetType().Name}.{input.MethodBase.Name}");

            for (var i = 0; i < input.Arguments.Count; i++)
            {
                if (i == 0)
                {
                    Logger.WriteLine("Parameters:");
                }

                Logger.WriteLine($"{input.Arguments.ParameterName(i)}: {input.Arguments[i]}");
            }

            if (result.Exception != null)
            {
                Logger.WriteLine($"Exception occured: {result.Exception.GetType()}");
            }

            Logger.WriteLine(string.Empty);

            return result;
        }
    }
}
