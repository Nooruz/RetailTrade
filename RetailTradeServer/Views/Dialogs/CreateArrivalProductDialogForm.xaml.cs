using RetailTradeServer.Dialogs;

namespace RetailTradeServer.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreateArrivalProductDialogForm.xaml
    /// </summary>
    public partial class CreateArrivalProductDialogForm : BaseDialogUserControl
    {
        public CreateArrivalProductDialogForm()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.Xpf.Grid.ExceptionMode.NoAction;
        }
    }
}
