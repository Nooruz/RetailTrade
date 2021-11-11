﻿using System;
using System.Collections.Generic;

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
        public decimal Sum { get; set; }

        /// <summary>
        /// Оплачено наличными
        /// </summary>
        public decimal PaidInCash { get; set; }

        /// <summary>
        /// Оплачено безналичными
        /// </summary>
        public decimal PaidInCashless { get; set; }

        /// <summary>
        /// Сдача
        /// </summary>
        public decimal Change { get; set; }        

        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift { get; set; }

        /// <summary>
        /// Товары
        /// </summary>
        public ICollection<ProductSale> ProductSales { get; set; }
    }
}
