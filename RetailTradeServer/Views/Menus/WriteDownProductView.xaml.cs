namespace RetailTradeServer.Views.Menus
{
    /// <summary>
    /// Interaction logic for WriteDownProductDialogForm.xaml
    /// </summary>
    public partial class WriteDownProductView : BaseMenuView
    {
        public WriteDownProductView()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
