namespace RetailTradeServer.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreateRefundToSupplierDialogForm.xaml
    /// </summary>
    public partial class CreateRefundToSupplierDialogForm : BaseDialogUserControl
    {
        public CreateRefundToSupplierDialogForm()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
