using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RetailTradeClient.Converters
{
    public class PercentageOrSomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDiscountPercentage)
            {
                return isDiscountPercentage ? "%" : "С";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
