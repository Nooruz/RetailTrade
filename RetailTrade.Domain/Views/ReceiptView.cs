using System;

namespace RetailTrade.Domain.Views
{
    public class ReceiptView : ViewObject
    {
        #region Private Members

        private int _shiftId;
        private int _userId;
        private int _pointSaleId;
        private DateTime _dateOfPurchase;
        private decimal _total;
        private decimal _paidInCash;
        private decimal _paidInCashless;
        private decimal _change;
        private string _kKMCheckNumber;
        private bool _isRefund;
        private decimal _discountAmount;
        private decimal _amountWithoutDiscount;
        private DateTime _openingDate;
        private DateTime? _closingDate;
        private decimal _sum;

        #endregion

        #region Public Properties

        public int ShiftId
        {
            get => _shiftId;
            set
            {
                _shiftId = value;
                OnPropertyChanged(nameof(ShiftId));
            }
        }
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        public int PointSaleId
        {
            get => _pointSaleId;
            set
            {
                _pointSaleId = value;
                OnPropertyChanged(nameof(PointSaleId));
            }
        }
        public DateTime DateOfPurchase
        {
            get => _dateOfPurchase;
            set
            {
                _dateOfPurchase = value;
                OnPropertyChanged(nameof(DateOfPurchase));
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
        public decimal PaidInCash
        {
            get => _paidInCash;
            set
            {
                _paidInCash = value;
                OnPropertyChanged(nameof(PaidInCash));
            }
        }
        public decimal PaidInCashless
        {
            get => _paidInCashless;
            set
            {
                _paidInCashless = value;
                OnPropertyChanged(nameof(PaidInCashless));
            }
        }
        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
            }
        }
        public string KKMCheckNumber
        {
            get => _kKMCheckNumber;
            set
            {
                _kKMCheckNumber = value;
                OnPropertyChanged(nameof(KKMCheckNumber));
            }
        }
        public bool IsRefund
        {
            get => _isRefund;
            set
            {
                _isRefund = value;
                OnPropertyChanged(nameof(IsRefund));
            }
        }
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                OnPropertyChanged(nameof(DiscountAmount));
            }
        }
        public decimal AmountWithoutDiscount
        {
            get => _amountWithoutDiscount;
            set
            {
                _amountWithoutDiscount = value;
                OnPropertyChanged(nameof(AmountWithoutDiscount));
            }
        }
        public DateTime OpeningDate
        {
            get => _openingDate;
            set
            {
                _openingDate = value;
                OnPropertyChanged(nameof(OpeningDate));
            }
        }
        public DateTime? ClosingDate
        {
            get => _closingDate;
            set
            {
                _closingDate = value;
                OnPropertyChanged(nameof(ClosingDate));
            }
        }
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        #endregion
    }
}
