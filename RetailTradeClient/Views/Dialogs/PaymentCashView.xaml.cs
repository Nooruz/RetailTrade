namespace RetailTradeClient.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для PaymentCashView.xaml
    /// </summary>
    public partial class PaymentCashView : BaseUserControl
    {
        public PaymentCashView()
        {
            InitializeComponent();
        }

        private void BaseDialogUserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Focus();
        }
    }
}
