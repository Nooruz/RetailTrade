using System.Windows;
using System.Windows.Controls;

namespace RetailTradeClient.Customs
{
    public class UnitTextBox : TextBox
    {
        public string UnitName
        {
            get { return (string)GetValue(UnitNameProperty); }
            set { SetValue(UnitNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnitName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitNameProperty =
            DependencyProperty.Register("UnitName", typeof(string), typeof(UnitTextBox), new PropertyMetadata(string.Empty));
    }
}
