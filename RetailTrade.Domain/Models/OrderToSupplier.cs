using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class OrderToSupplier : DomainObject
    {
        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderProductId { get; set; }
        public string Comment { get; set; }
        public Supplier Supplier { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
