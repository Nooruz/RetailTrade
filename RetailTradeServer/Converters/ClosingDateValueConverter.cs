using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailTradeServer.Converters
{
    public class ClosingDateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "Открыта" : "Закрыта";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
