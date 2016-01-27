using System;
using System.Globalization;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View.Converter
{
    /// <summary>
    /// Converter from System.Drawing.Color to System.Windows.Media.Color.
    /// </summary>
    public sealed class DrawingColorToMediaColorConverter : IValueConverter
    {

        /// <summary>
        /// Right converter.
        /// </summary>
        /// <param name="value">Unconverted value. </param>
        /// <param name="targetType">Unconverted value type. </param>
        /// <param name="parameter">Appended parameters. </param>
        /// <param name="culture">Culture information. </param>
        /// <returns>Converted value. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is System.Drawing.Color)
            {
                var color = (System.Drawing.Color) value;
                return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            return null;
        }

        /// <summary>
        /// Reverse converter.
        /// </summary>
        /// <param name="value">Unconverted value. </param>
        /// <param name="targetType">Unconverted value type. </param>
        /// <param name="parameter">Appended parameters. </param>
        /// <param name="culture">Culture information. </param>
        /// <returns>Converted value. </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Media.Color)
            {
                var color = (System.Windows.Media.Color)value;
                return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            return null;

        }
    }
}
