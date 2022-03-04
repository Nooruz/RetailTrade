using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PostponeReceiptViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly HomeViewModel _viewModel;
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
            IProductService productService)
        {
            _viewModel = viewModel;
            _productService = productService;

            PostponeReceipts = _viewModel.PostponeReceipts;
            ResumeReceiptCommand = new RelayCommand(ResumeReceipt);
            RowDoubleClickCommand = new RelayCommand(ResumeReceipt);
        }

        #endregion

        #region Private Voids

        private async void ResumeReceipt()
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
                            Sum = postponeProduct.Sum,
                            QuantityInStock = await _productService.GetQuantity(postponeProduct.Id)
                        });
                    }
                    _viewModel.PostponeReceipts.Remove(SelectedPostponeReceipt);
                    Result = true;
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
