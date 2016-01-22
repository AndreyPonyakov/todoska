using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace TodoSystem.UI.Tools.View
{
    /// <summary>
    /// Standard colors.
    /// </summary>
    public sealed class StandardColors
    {
        /// <summary>
        /// Get a Color list
        /// </summary>
        public static IEnumerable<Color> Items { get; }

        /// <summary>
        /// Create an instance color list.
        /// </summary>
        static StandardColors()
        {
            Items = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<Color>()
                .ToList();
        }
    }
}
