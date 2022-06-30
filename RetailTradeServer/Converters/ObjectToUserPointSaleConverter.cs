using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RetailTradeServer.Converters
{
    public class ObjectToUserPointSaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is List<UserPointSale> userPointsSales)
                {
                    var users = userPointsSales.Select(u => u.User).ToList();
                    return users;
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is List<object> users)
                {
                    return users.Select(u => new UserPointSale { UserId = ((User)u).Id, User = (User)u }).ToList();
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return Binding.DoNothing;
        }
    }
}
