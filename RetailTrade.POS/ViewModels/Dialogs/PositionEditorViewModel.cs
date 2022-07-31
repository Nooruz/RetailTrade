using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Windows;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class PositionEditorViewModel : BaseViewModel
    {
        #region Private Members

        private ProductSale _editProductSale;
        private ProductView _product;
        private bool _isDiscountPercentage = true;
        private double _discount;
        private double _quantity;
        private decimal _retailPrice;
        private decimal _discountSum;
        private double _rest;

        #endregion

        #region Public Properties

        public Visibility DiscountVisibility => Product.ProhibitDiscount ? Visibility.Collapsed : Visibility.Visible;
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
        public ProductView Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
                OnPropertyChanged(nameof(DiscountVisibility));
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
                _discountSum = (decimal)Discount * Total;
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(DiscountSum));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(TotalWithDiscount));
            }
        }
        public decimal DiscountSum
        {
            get => _discountSum;
            set
            {
                _discountSum = value;
                _discount = (double)Math.Round(DiscountSum / Total, 2);
                OnPropertyChanged(nameof(DiscountSum));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(TotalWithDiscount));
            }
        }
        public decimal Total => (decimal)Quantity * RetailPrice;
        public decimal TotalWithDiscount => Total - DiscountSum;
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(TotalWithDiscount));
                OnPropertyChanged(nameof(Rest));
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
                OnPropertyChanged(nameof(TotalWithDiscount));
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

        #endregion

        #region Action

        public event Action<ProductSale>? OnDeleteProductSale;

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
            EditProductSale.TotalWithDiscount = TotalWithDiscount;
            EditProductSale.DiscountAmount = DiscountSum;
            CurrentWindowService.Close();
        }

        [Command]
        public void Delete()
        {            
            OnDeleteProductSale?.Invoke(EditProductSale);
            CurrentWindowService.Close();
        }

        #endregion
    }
}
