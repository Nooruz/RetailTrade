using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using System;
using System.Linq;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class DiscountReceiptViewModel : BaseViewModel
    {
        #region Private Members

        private bool _isDiscountPercentage = true;
        private double _discount;
        private decimal _discountSum;
        private decimal _total;
        private Receipt _receipt = new();

        #endregion

        #region Public Properties

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
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }
        public decimal TotalWithDiscount => Total - DiscountSum;
        public Receipt Receipt
        {
            get => _receipt;
            set
            {
                _receipt = value;
                Total = Receipt.Total;
                DiscountSum = Receipt.DiscountAmount;
                OnPropertyChanged(nameof(Receipt));
                OnPropertyChanged(nameof(TotalWithDiscount));
            }
        }

        #endregion

        #region Action

        public event Action<decimal>? OnDiscountChanged;
        public event Action? OnDeleteDiscountReceipt;

        #endregion

        #region Public Voids

        [Command]
        public void Save()
        {
            OnDiscountChanged?.Invoke(DiscountSum);
            CurrentWindowService.Close();
        }

        [Command]
        public void Delete()
        {
            OnDeleteDiscountReceipt?.Invoke();
            CurrentWindowService.Close();
        }

        #endregion
    }
}
