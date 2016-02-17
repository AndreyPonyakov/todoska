using System;
using System.Globalization;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View.Converter
{
    /// <summary>
    /// Class to the argument converter using boolean implication operation.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanImplicationConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.If the method returns null, the valid null value is used. </returns>
        public object Convert(
            object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            try
            {
                return !((bool)values[0]) || ((bool)values[1]);
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.  </returns>
        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
