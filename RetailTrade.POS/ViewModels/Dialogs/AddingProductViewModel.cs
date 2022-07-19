using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class AddingProductViewModel : BaseViewModel
    {
        #region Private Members

        private ProductSale _editProductSale = new();
        private double _rest;
        private double _quantity;

        #endregion

        #region Public Properties

        public ProductSale EditProductSale
        {
            get => _editProductSale;
            set
            {
                _editProductSale = value;
                OnPropertyChanged(nameof(EditProductSale));
            }
        }
        public double Rest
        {
            get => _rest - Quantity;
            set
            {
                _rest = value;
                OnPropertyChanged(nameof(Rest));
            }
        }
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                EditProductSale.Quantity = Quantity;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Rest));
            }
        }

        #endregion

        #region Action

        public event Action<ProductSale>? OnProductSaleAdding;

        #endregion

        #region Public Voids

        [Command]
        public void Add()
        {
            OnProductSaleAdding?.Invoke(EditProductSale);
            CurrentWindowService.Close();
        }

        #endregion
    }
}
