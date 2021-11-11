using DevExpress.Xpf.Grid;
using System;
using System.Windows;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private void TableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
        {
            var tableView = sender as TableView;
            Dispatcher.BeginInvoke((Action)tableView.Grid.View.ShowEditor, DispatcherPriority.Input);
            tableView.Grid.CurrentColumn = tableView.Grid.Columns[2];
            //var cell = tableView.Grid.cell;
        }
    }
}
