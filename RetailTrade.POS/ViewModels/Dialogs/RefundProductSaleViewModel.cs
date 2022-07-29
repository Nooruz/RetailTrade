using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class RefundProductSaleViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductSaleService _productSaleService;
        private ReceiptView _editedReceipt;
        private ObservableCollection<ProductSaleView> _productSales = new();
        private decimal _summary;
        private decimal _discount;

        #endregion

        #region Public Properties

        public decimal Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }
        public decimal Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }
        public ReceiptView EditedReceipt
        {
            get => _editedReceipt;
            set
            {
                _editedReceipt = value;
                GetData(EditedReceipt.Id);
                OnPropertyChanged(nameof(EditedReceipt));
                OnPropertyChanged(nameof(Summary));
                OnPropertyChanged(nameof(Discount));
            }
        }
        public ObservableCollection<ProductSaleView> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                OnPropertyChanged(nameof(ProductSales));
            }
        }

        #endregion

        #region Constructor

        public RefundProductSaleViewModel(IProductSaleService productSaleService)
        {
            _productSaleService = productSaleService;
        }

        #endregion

        #region Private Voids

        private async void GetData(int receiptId)
        {
            ProductSales = new(await _productSaleService.GetAllAsync(receiptId));
            if (ProductSales != null && ProductSales.Any())
            {
                Summary = ProductSales.Sum(p => p.Total);
                Discount = EditedReceipt.DiscountAmount + ProductSales.Sum(p => p.DiscountAmount);
            }
            else
            {
                Summary = EditedReceipt.Total;
                Discount = EditedReceipt.DiscountAmount;
            }
        }

        #endregion

        #region Public Properties

        [Command]
        public void Refund()
        {
            try
            {
                DocumentViewerService.Show(nameof(RefundEditorView), new RefundEditorViewModel()
                {
                    Title = "Возврат",
                    ProductSales = ProductSales
                });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
