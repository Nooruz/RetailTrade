using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Смена
    /// </summary>
    public class Shift : DomainObject
    {
        /// <summary>
        /// Дата открытия смены
        /// </summary>
        public DateTime OpeningDate { get; set; }

        /// <summary>
        /// Дата закрытия смены
        /// </summary>
        public DateTime? ClosingDate { get; set; }

        /// <summary>
        /// Код кассира
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Кассир
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Список чеков
        /// </summary>
        public IEnumerable<Receipt> Receipts { get; set; }
    }
}
