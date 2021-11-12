using RetailTradeServer.Dialogs;

namespace RetailTradeServer.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderToSupplierDialogForm.xaml
    /// </summary>
    public partial class CreateOrderToSupplierDialogForm : BaseDialogUserControl
    {
        public CreateOrderToSupplierDialogForm()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
