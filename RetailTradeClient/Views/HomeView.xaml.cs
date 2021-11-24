using DevExpress.Xpf.Grid;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RetailTradeClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void TableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
        {
            var tableView = sender as TableView;
            Dispatcher.BeginInvoke((Action)tableView.Grid.View.ShowEditor, DispatcherPriority.Input);
            //tableView.Grid.CurrentColumn = tableView.Grid.Columns[2];
        }

        private void TableView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }
}
