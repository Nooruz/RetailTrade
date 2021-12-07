using DevExpress.Xpf.Grid;
using RetailTradeClient.Dialogs;
using System;
using System.Windows.Threading;

namespace RetailTradeClient.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для PaymentCashlessView.xaml
    /// </summary>
    public partial class PaymentComplexView : BaseDialogUserControl
    {
        public PaymentComplexView()
        {
            InitializeComponent();
        }

        private void TableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            TableView tableView = sender as TableView;
            tableView.Grid.CurrentColumn = tableView.Grid.Columns[1];
            Dispatcher.BeginInvoke(new Action(() => 
            {
                tableView.ShowEditor();
            }), DispatcherPriority.Render);
        }
    }
}
