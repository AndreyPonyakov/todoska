using Microsoft.Practices.Unity;

namespace TodoSystem.Service.Tools.UnityExtension
{
    /// <summary>
    /// Abstract class for disposing manager family.
    /// </summary>
    public abstract class DisposingLifetimeManager : LifetimeManager
    {
        /// <summary>
        /// Applies manager policy to the current instance.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        /// <returns>True if appliance was success. </returns>
        public abstract bool AppliesTo(object instance);

        /// <summary>
        /// Detaches current instance from manager policy.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        public abstract void RemoveValue(object instance);
    }
}