using System;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View.Converter
{
    /// <summary>
    /// Converter for conditions of state pattern.
    /// </summary>
    public sealed class StatePatternToBooleanConverter : IMultiValueConverter
    {
        /// <summary>
        /// Right converter.
        /// </summary>
        /// <param name="value">Unconverted value. </param>
        /// <param name="targetType">Unconverted value type. </param>
        /// <param name="parameter">Appended parameters. </param>
        /// <param name="culture">Culture information. </param>
        /// <returns>Converted value. </returns>
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Length >= 2 &&
                value[0] != null && value[1] != null &&
                value[0] == value[1];
        }

        /// <summary>
        /// Reverse converter.
        /// </summary>
        /// <param name="value">Unconverted value. </param>
        /// <param name="targetType">Unconverted value type. </param>
        /// <param name="parameter">Appended parameters. </param>
        /// <param name="culture">Culture information. </param>
        /// <returns>Converted value. </returns>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
