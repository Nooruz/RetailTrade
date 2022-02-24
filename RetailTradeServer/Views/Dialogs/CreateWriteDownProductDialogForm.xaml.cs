namespace RetailTradeServer.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreateWriteDownProductDialogForm.xaml
    /// </summary>
    public partial class CreateWriteDownProductDialogForm : BaseDialogUserControl
    {
        public CreateWriteDownProductDialogForm()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
