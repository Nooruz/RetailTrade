using RetailTrade.SQLServerConnectionDialog.ViewModels;
using System.Windows;

namespace RetailTrade.SQLServerConnectionDialog
{
    public static class ConnectionDialog
    {
        #region Private Static Members

        private static MainWindow _window;
        private static ConnectionViewModel _viewModel;

        #endregion

        #region Constructr

        static ConnectionDialog()
        {
            _window = new();
            _viewModel = new(_window);
            _window.CurrentViewModel = _viewModel;
        }

        #endregion

        #region Private Static Members

        public static string ConnectionString { get; private set; }

        #endregion

        #region Public Static Voids

        public static MessageBoxResult Show()
        {
            _ = (_window?.ShowDialog());
            ConnectionString = _viewModel.ConnectionStringBuilder.ConnectionString;
            return _viewModel.Result;
        }

        #endregion
    }
}
