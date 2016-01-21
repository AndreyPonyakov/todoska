using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Todo.UI.Tools.View
{
    /// <summary>
    /// Standard colors.
    /// </summary>
    public class StandardColors
    {
        /// <summary>
        /// Color list.
        /// </summary>
        public static IEnumerable<Color> Items { get; }

        /// <summary>
        /// Create instance color list.
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
