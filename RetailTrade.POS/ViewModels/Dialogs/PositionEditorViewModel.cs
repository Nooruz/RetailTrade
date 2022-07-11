using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class PositionEditorViewModel : BaseViewModel
    {
        #region Private Members

        private ProductSale _editProductSale;
        private bool _isDiscountPercentage = true;
        private double _discount;
        private double _quantity;
        private decimal _retailPrice;
        private decimal _discountSum;

        #endregion

        #region Public Properties

        public ProductSale EditProductSale
        {
            get => _editProductSale;
            set
            {
                _editProductSale = value;
                Quantity = EditProductSale.Quantity;
                RetailPrice = EditProductSale.RetailPrice;
                DiscountSum = EditProductSale.DiscountAmount;
                OnPropertyChanged(nameof(EditProductSale));
            }
        }
        public bool IsDiscountPercentage
        {
            get => _isDiscountPercentage;
            set
            {
                _isDiscountPercentage = value;
                OnPropertyChanged(nameof(IsDiscountPercentage));
                OnPropertyChanged(nameof(DiscountIcon));
            }
        }
        public string DiscountIcon => IsDiscountPercentage ? string.Empty : "Сом";
        public double Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                _discountSum = (decimal)Discount * RetailPrice;
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(DiscountSum));
                OnPropertyChanged(nameof(Total));
            }
        }
        public decimal DiscountSum
        {
            get => _discountSum;
            set
            {
                _discountSum = value;
                _discount = (double)Math.Round(DiscountSum / RetailPrice, 2);
                OnPropertyChanged(nameof(DiscountSum));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Discount));
            }
        }
        public decimal Total => ((decimal)Quantity * RetailPrice) - DiscountSum;
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Total));
            }
        }
        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
                OnPropertyChanged(nameof(Total));
            }
        }

        #endregion

        #region Private Voids



        #endregion

        #region Public Properties

        [Command]
        public void Save()
        {
            EditProductSale.Quantity = Quantity;
            EditProductSale.RetailPrice = RetailPrice;
            EditProductSale.Total = Total;
            EditProductSale.DiscountAmount = DiscountSum;
            CurrentWindowService.Close();
        }

        #endregion
    }
}
