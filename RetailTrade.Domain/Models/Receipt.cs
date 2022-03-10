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
        private decimal _sum;
        private decimal _paidInCash;
        private decimal _paidInCashless;
        private decimal _change;
        private decimal _deposited;
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
        /// Сумма квитанции
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
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
        /// Внесено
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deposited
        {
            get => _deposited;
            set
            {
                _deposited = value;
                OnPropertyChanged(nameof(Deposited));
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
