using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class RefundEditorViewModel : BaseViewModel
    {
        #region Private Members

        private string _summary;
        private ObservableCollection<ProductSaleView> _productSales = new();
        private Refund _refund;

        #endregion

        #region Public Properties

        public ObservableCollection<ProductSaleView> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                Summary = $"К возврату: {ProductSales.Sum(p => p.TotalWithDiscount):N2}";
                OnPropertyChanged(nameof(ProductSales));
            }
        }
        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }

        #endregion
    }
}
