using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class Refund : DomainObject
    {
        #region Private Members

        private DateTime _dateOfRefund;
        private int _shiftId;
        private decimal _sum;
        private int? _receiptId;

        #endregion
        /// <summary>
        /// Дата возврата
        /// </summary>
        public DateTime DateOfRefund
        {
            get => _dateOfRefund;
            set
            {
                _dateOfRefund = value;
                OnPropertyChanged(nameof(DateOfRefund));
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

        public int? ReceiptId
        {
            get => _receiptId;
            set
            {
                _receiptId = value;
                OnPropertyChanged(nameof(ReceiptId));
            }
        }

        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift { get; set; }

        public Receipt Receipt { get; set; }

        /// <summary>
        /// Товары возврата
        /// </summary>
        public ICollection<ProductRefund> ProductRefunds { get; set; }
    }
}
