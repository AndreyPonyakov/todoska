using System.ServiceModel;

using Microsoft.Practices.Unity;

namespace TodoSystem.Service.Host
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
        /// <param name="container">Unity container to manage instance lifetime. </param>
        public static void SetFactory<T>(this ServiceHost serviceHost, IUnityContainer container)
            where T : class
        {
            var behavior = new InstanceProviderBehavior<T>(container);
            behavior.AddToAllContracts(serviceHost);
        }
    }
}
