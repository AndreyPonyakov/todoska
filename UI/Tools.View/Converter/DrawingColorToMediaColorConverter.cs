using System;
using System.Globalization;
using System.Windows.Data;

namespace Todo.UI.Tools.View.Converter
{
    public class DrawingColorToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is System.Drawing.Color)
            {
                var color = (System.Drawing.Color) value;
                return System.Windows.Media.Color.FromScRgb(color.A, color.R, color.G, color.B);
            }
            return null;
        }

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
