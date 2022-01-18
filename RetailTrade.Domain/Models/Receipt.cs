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
        /// <summary>
        /// Дата и время покупки
        /// </summary>
        public DateTime DateOfPurchase { get; set; }

        /// <summary>
        /// Код смены
        /// </summary>
        public int ShiftId { get; set; }

        /// <summary>
        /// Сумма квитанции
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }

        /// <summary>
        /// Оплачено наличными
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidInCash { get; set; }

        /// <summary>
        /// Оплачено безналичными
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidInCashless { get; set; }

        /// <summary>
        /// Сдача
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Change { get; set; }        

        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift { get; set; }

        public string KKMCheckNumber { get; set; }

        public bool IsRefund { get; set; }

        /// <summary>
        /// Товары
        /// </summary>
        public List<ProductSale> ProductSales { get; set; }
    }
}
