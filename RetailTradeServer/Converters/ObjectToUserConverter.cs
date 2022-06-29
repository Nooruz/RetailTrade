using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RetailTradeServer.Converters
{
    public class ObjectToUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is List<object> users)
                {
                    return users.Select(u => (User)u).ToList();
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
