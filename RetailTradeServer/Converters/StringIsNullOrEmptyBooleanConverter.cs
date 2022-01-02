using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailTradeServer.Converters
{
    public class StringIsNullOrEmptyBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = string.IsNullOrEmpty(value.ToString());
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
