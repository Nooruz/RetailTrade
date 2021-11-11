using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class SaleViewModel : BaseViewModel
    {
        #region Private Members

        private string _searchText;

        #endregion

        #region Public Properties

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string TotalToBePaid => string.Format("{0:#,##0.00}", 0);

        #endregion

        #region Commands

        public ICommand CleareSearchTextCommand { get; }

        #endregion

        #region Constructor

        public SaleViewModel()
        {
            CleareSearchTextCommand = new RelayCommand(CleareSearchText);
        }

        #endregion

        #region Private Voids

        private void CleareSearchText()
        {
            SearchText = string.Empty;
        }

        #endregion
    }
}
