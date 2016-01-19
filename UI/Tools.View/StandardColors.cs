using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Todo.UI.Tools.View
{
    public class StandardColors
    {
        public static IEnumerable<Color> Items { get; }

        static StandardColors()
        {
            Items = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<Color>()
                .ToList();
        }
    }
}
