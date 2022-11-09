using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailTradeServer.Converters
{
    public class DoublePositiveToNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (double.TryParse(value.ToString(), out double n))
                {
                    return Math.Abs(n);
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (double.TryParse(value.ToString(), out double n))
                {
                    return n *= -1;
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }
    }
}
