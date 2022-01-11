using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Смена
    /// </summary>
    public class Shift : DomainObject
    {
        #region Private Members

        private DateTime _openingDate;
        private DateTime? _closingDate;
        private int _userId;
        private User _user;
        private decimal _sum;

        #endregion

        #region Public Properties

        /// <summary>
        /// Дата открытия смены
        /// </summary>
        public DateTime OpeningDate
        {
            get => _openingDate;
            set
            {
                _openingDate = value;
                OnPropertyChanged(nameof(OpeningDate));
            }
        }

        /// <summary>
        /// Дата закрытия смены
        /// </summary>
        public DateTime? ClosingDate
        {
            get => _closingDate;
            set
            {
                _closingDate = value;
                OnPropertyChanged(nameof(ClosingDate));
            }
        }

        /// <summary>
        /// Код кассира
        /// </summary>
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        /// <summary>
        /// Кассир
        /// </summary>
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        /// <summary>
        /// Сумма
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
        /// Список чеков
        /// </summary>
        public IEnumerable<Receipt> Receipts { get; set; }

        /// <summary>
        /// Список возвратов
        /// </summary>
        public IEnumerable<Refund> Refunds { get; set; }

        #endregion
    }
}
