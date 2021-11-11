namespace RetailTradeServer.Views.Menus
{
    /// <summary>
    /// Логика взаимодействия для RefundToSupplierView.xaml
    /// </summary>
    public partial class RefundToSupplierView : BaseMenuView
    {
        public RefundToSupplierView()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
