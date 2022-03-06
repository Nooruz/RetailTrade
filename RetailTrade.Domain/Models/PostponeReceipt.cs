using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Отложенный чек
    /// </summary>
    public class PostponeReceipt
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Sum { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
