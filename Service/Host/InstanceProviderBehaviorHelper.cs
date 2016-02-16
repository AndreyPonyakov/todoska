using System;
using System.ServiceModel;

namespace Host
{
    /// <summary>
    /// Syntactic sugar to the <see cref="InstanceProviderBehavior{T}"/> class.
    /// </summary>
    public static class InstanceProviderBehaviorHelper
    {
        /// <summary>
        /// Appends instance builder to all contracts of host.
        ///  </summary>
        /// <typeparam name="T">Service type. </typeparam>
        /// <param name="serviceHost">Target host. </param>
        /// <param name="instanceProvider">Instance creation factory. </param>
        public static void SetFactory<T>(this ServiceHost serviceHost, Func<T> instanceProvider)
            where T : class
        {
            var behavior = new InstanceProviderBehavior<T>(instanceProvider);
            behavior.AddToAllContracts(serviceHost);
        }
    }
}
