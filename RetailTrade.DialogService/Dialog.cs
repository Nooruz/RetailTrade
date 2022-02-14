using System.Windows;
using System.Windows.Controls;

namespace RetailTrade.DialogService
{
    public class Dialog : Control
    {
        static Dialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog), new FrameworkPropertyMetadata(typeof(Dialog)));
        }
    }
}
