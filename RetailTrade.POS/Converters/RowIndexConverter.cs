using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailTrade.POS.Converters
{
    public class RowIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ((int)values[1] - (int)values[0]).ToString();
            }
            catch (Exception)
            {
                //ignore
            }
            return "0";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
