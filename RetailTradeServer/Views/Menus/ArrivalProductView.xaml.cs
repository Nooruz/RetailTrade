namespace RetailTradeServer.Views.Menus
{
    /// <summary>
    /// Interaction logic for ArrivalProduct.xaml
    /// </summary>
    public partial class ArrivalProductView : BaseMenuView
    {
        public ArrivalProductView()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
