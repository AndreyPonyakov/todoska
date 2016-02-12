using System;

namespace TodoSystem.Service.Tools.UnityExtension
{
    /// <summary>
    /// Shared strategy of instance lifetime.
    /// </summary>
    public sealed class DisposingSharedLifetimeManager : DisposingLifetimeManager, IDisposable
    {
        private object _object;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns> the object desired, or null if no such object is currently stored. </returns>
        public override object GetValue()
        {
            return _object;
        }

        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        public override void RemoveValue()
        {
            _object = null;
        }

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            _object = newValue;
        }

        /// <summary>
        /// Applies manager policy to the current instance.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        /// <returns>True if appliance was success. </returns>
        public override bool AppliesTo(object instance)
        {
            return instance == _object;
        }

        /// <summary>
        /// Detaches current instance from manager policy.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        public override void RemoveValue(object instance)
        {
            RemoveValue();
        }
    }
}