using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Refund : DomainObject
    {
        /// <summary>
        /// Дата возврата
        /// </summary>
        public DateTime DateOfRefund { get; set; }

        /// <summary>
        /// Код смены
        /// </summary>
        public int ShiftId { get; set; }

        /// <summary>
        /// Сумма квитанции
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift { get; set; }

        /// <summary>
        /// Товары возврата
        /// </summary>
        public ICollection<ProductRefund> ProductRefunds { get; set; }
    }
}
