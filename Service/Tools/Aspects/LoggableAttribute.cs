using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace TodoSystem.Service.Tools.Aspects
{
    /// <summary>
    /// Attribute class of a loggable behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LoggableAttribute : HandlerAttribute
    {
        /// <summary>
        /// Derived classes implement this method. When called, it creates a new call handler as specified in the attribute configuration.
        /// </summary>
        /// <param name="container">The container to use when creating handlers, if necessary.</param>
        /// <returns>A new call handler object. </returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<LoggableCallHandler>();
        }
    }
}
