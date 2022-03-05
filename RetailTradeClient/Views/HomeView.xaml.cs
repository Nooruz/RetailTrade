using DevExpress.Xpf.Grid;
using RetailTradeClient.Views.Dialogs;

namespace RetailTradeClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : BaseUserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void TableView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }
}
