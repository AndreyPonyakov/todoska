using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoSystem.Service.Tools.UnityExtension
{
    /// <summary>
    /// Transient strategy of instance lifetime.
    /// </summary>
    public sealed class DisposingTransientLifetimeManager : DisposingLifetimeManager, IDisposable
    {
        private readonly List<WeakReference> _values = new List<WeakReference>();

        /// <summary>
        /// Applies manager policy to the current instance.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        /// <returns>True if appliance was success. </returns>
        public override bool AppliesTo(object instance)
        {
            return _values.Any(wr => wr.Target == instance);
        }

        /// <summary>
        /// Detaches current instance from manager policy.
        /// </summary>
        /// <param name="instance">Targeting instance. </param>
        public override void RemoveValue(object instance)
        {
            var value = _values.FirstOrDefault(v => v.Target == instance);
            if (value != null)
            {
                _values.Remove(value);
            }

            RemoveDeadReferences();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _values.Clear();
        }

        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns> the object desired, or null if no such object is currently stored. </returns>
        public override object GetValue()
        {
            return null;
        }

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            RemoveDeadReferences();
            _values.Add(new WeakReference(newValue));
        }

        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        public override void RemoveValue()
        {
        }

        private void RemoveDeadReferences()
        {
            var value = _values.FirstOrDefault(v => !v.IsAlive);
            if (value != null)
            {
                _values.Remove(value);
            }
        }
    }
}