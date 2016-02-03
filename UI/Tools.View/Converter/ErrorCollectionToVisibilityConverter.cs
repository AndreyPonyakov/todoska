using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TodoSystem.UI.Tools.View.Converter
{
    /// <summary>
    /// Class-converter to hide empty error's list.
    /// </summary>
    public sealed class ErrorCollectionToVisibilityConverter : IValueConverter
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
            var collection = value as ReadOnlyObservableCollection<ValidationError>;
            if (collection == null || !collection.Any())
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
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
            throw new NotImplementedException();
        }
    }
}
