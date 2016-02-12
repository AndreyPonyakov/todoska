using System;
using System.Collections.Generic;

namespace TodoSystem.Service.Tools.UnityExtension
{
    /// <summary>
    /// Implementing comparison fot weak reference instances.
    /// </summary>
    public class WeakReferenceComparer : IEqualityComparer<WeakReference>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(WeakReference x, WeakReference y)
        {
            return x.IsAlive && y.IsAlive && x.Target == y.Target;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(WeakReference obj)
        {
            return obj.IsAlive ? obj.Target.GetHashCode() : GetHashCode();
        }
    }
}
