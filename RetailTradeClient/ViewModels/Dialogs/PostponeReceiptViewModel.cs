using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PostponeReceiptViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly HomeViewModel _viewModel;
        private readonly IUIManager _manager;
        private PostponeReceipt _selectedPostponeReceipt;

        #endregion

        #region Public Properties

        public List<PostponeReceipt> PostponeReceipts { get; set; }
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

        public ICommand ResumeReceiptCommand { get; }
        public ICommand RowDoubleClickCommand { get; }

        #endregion

        #region Constructor

        public PostponeReceiptViewModel(HomeViewModel viewModel,
            IUIManager manager)
        {
            _viewModel = viewModel;
            _manager = manager;

            PostponeReceipts = _viewModel.PostponeReceipts;
            ResumeReceiptCommand = new RelayCommand(ResumeReceipt);
            RowDoubleClickCommand = new RelayCommand(ResumeReceipt);
        }

        #endregion

        #region Private Voids

        private void ResumeReceipt()
        {
            if (SelectedPostponeReceipt != null)
            {
                try
                {
                    
                    foreach (PostponeProduct postponeProduct in SelectedPostponeReceipt.PostponeProducts)
                    {
                        _viewModel.SaleProducts.Add(new Sale
                        {
                            Id = postponeProduct.Id,
                            Name = postponeProduct.Name,
                            SalePrice = postponeProduct.SalePrice,
                            Quantity = postponeProduct.Quantity,
                            Sum = postponeProduct.Sum
                        });
                    }
                    _viewModel.PostponeReceipts.Remove(SelectedPostponeReceipt);
                    Result = true;
                    _manager.Close();
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
