using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailTradeClient.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double width && width != 0 ? width / 6 : (object)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
