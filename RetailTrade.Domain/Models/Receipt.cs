using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{

    /// <summary>
    /// Квитания
    /// </summary>
    public class Receipt : DomainObject
    {
        #region Private Members

        private DateTime _dateOfPurchase;
        private int _shiftId;
        private decimal _amountWithoutDiscount;
        private decimal _total;
        private decimal _paidInCash;
        private decimal _paidInCashless;
        private decimal _change;
        private decimal _discountAmount;
        private string _kKMCheckNumber;
        private bool _isRefund;
        private Shift _shift;

        #endregion

        #region Public Properties


        /// <summary>
        /// Дата и время покупки
        /// </summary>
        public DateTime DateOfPurchase
        {
            get => _dateOfPurchase;
            set
            {
                _dateOfPurchase = value;
                OnPropertyChanged(nameof(DateOfPurchase));
            }
        }

        /// <summary>
        /// Код смены
        /// </summary>
        public int ShiftId
        {
            get => _shiftId;
            set
            {
                _shiftId = value;
                OnPropertyChanged(nameof(ShiftId));
            }
        }

        /// <summary>
        /// Сумма без скидки
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountWithoutDiscount
        {
            get => _amountWithoutDiscount;
            set
            {
                _amountWithoutDiscount = value;
                OnPropertyChanged(nameof(AmountWithoutDiscount));
            }
        }

        /// <summary>
        /// Итого к оплате
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        /// <summary>
        /// Сумма скидки
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                OnPropertyChanged(nameof(DiscountAmount));
            }
        }

        /// <summary>
        /// Оплачено наличными
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidInCash
        {
            get => _paidInCash;
            set
            {
                _paidInCash = value;
                OnPropertyChanged(nameof(PaidInCash));
            }
        }

        /// <summary>
        /// Оплачено безналичными
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidInCashless
        {
            get => _paidInCashless;
            set
            {
                _paidInCashless = value;
                OnPropertyChanged(nameof(PaidInCashless));
            }
        }

        /// <summary>
        /// Сдача
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
            }
        }

        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift
        {
            get => _shift;
            set
            {
                _shift = value;
                OnPropertyChanged(nameof(Shift));
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

        /// <summary>
        /// Товары
        /// </summary>
        public List<ProductSale> ProductSales { get; set; }

        #endregion
    }
}
