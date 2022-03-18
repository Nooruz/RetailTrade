using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using RetailTradeClient.State.ProductSales;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PostponeReceiptViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductSaleStore _productSaleStore;
        private PostponeReceipt _selectedPostponeReceipt;

        #endregion

        #region Public Properties

        public ObservableCollection<PostponeReceipt> PostponeReceipts => _productSaleStore.PostponeReceipts;
        public PostponeReceipt SelectedPostponeReceipt
        {
            get => _selectedPostponeReceipt;
            set
            {
                _selectedPostponeReceipt = value;
                OnPropertyChanged(nameof(SelectedPostponeReceipt));
            }
        }

        #endregion

        #region Commands

        public ICommand ResumeReceiptCommand => new RelayCommand(ResumeReceipt);
        public ICommand RowDoubleClickCommand => new RelayCommand(ResumeReceipt);

        #endregion

        #region Constructor

        public PostponeReceiptViewModel(IProductSaleStore productSaleStore)
        {
            _productSaleStore = productSaleStore;
        }

        #endregion

        #region Private Voids

        private void ResumeReceipt()
        {
            if (SelectedPostponeReceipt != null)
            {
                try
                {
                    _productSaleStore.ResumeReceipt(SelectedPostponeReceipt.Id);
                    CurrentWindowService.Close();
                }
                catch (Exception)
                {
                    //ignore
                }                
            }
        }

        #endregion
    }
}
