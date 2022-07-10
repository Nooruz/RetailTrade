using System.Windows;
using System.Windows.Controls;

namespace RetailTrade.POS.Views
{
    public class BaseView : UserControl
    {
        public BaseView()
        {
            Style = FindResource("BaseView") as Style;
        }
    }
}
