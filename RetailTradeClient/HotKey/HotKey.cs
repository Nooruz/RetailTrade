using System.Windows.Input;

namespace RetailTradeClient.HotKey
{
    public class HotKey
    {
        public static RoutedCommand CashlessPaymentCommand = new();

        public HotKey()
        {
            CashlessPaymentCommand.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.None));
        }
    }
}
